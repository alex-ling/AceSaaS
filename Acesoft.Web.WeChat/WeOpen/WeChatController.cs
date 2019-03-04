using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

using Acesoft.Data;
using Acesoft.Rbac;
using Acesoft.Web.WeChat.Entity;
using Acesoft.Web.WeChat.WeOpen;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Senparc.Weixin.Exceptions;
using Senparc.Weixin.MP.Entities.Request;
using Senparc.Weixin.MP.MvcExtension;
using Senparc.Weixin.WxOpen.Containers;
using Senparc.Weixin.WxOpen.Helpers;

namespace Acesoft.Web.WeChat
{
    [ApiExplorerSettings(GroupName = "plat")]
	[Route("api/[controller]/[action]")]
	public class WeChatController : Controller
	{
        private IApplicationContext appCtx;
        private IAppService appService;
        private IMenuService menuService;
        private IMediaService mediaService;
        private INewsService newsService;
        private Wx_App app;

		public WeChatController(
            IApplicationContext appCtx,
            IWeChatContainer container,
            IAppService appService,
            IMenuService menuService,
            IMediaService mediaService,
            INewsService newsService)
		{
            this.appCtx = appCtx;
            this.app = container.GetApp();
			this.appService = appService;
            this.menuService = menuService;
            this.mediaService = mediaService;
            this.newsService = newsService;
		}

		[HttpGet]
		public IActionResult GetToken()
		{
			var secret = App.GetQuery("secret", "");
            return Ok(appService.GetToken(app, secret));
		}

		[HttpPost]
		public async Task<IActionResult> Login([FromBody] JObject data)
		{
			var userName = data.GetValue<string>("username");
			var password = data.GetValue<string>("password");
			var openId = data.GetValue<string>("openid");
            await appCtx.AccessControl.WechatLogin(userName, password, openId);

			return Ok(null);
		}

        #region api
        [HttpGet, Route("/api/wechat")]
		public IActionResult Get(PostModel model, string echostr)
		{
			if (appService.Check(app, model))
			{
				return Content(echostr);
			}
			return Content("WeChatOpen服务验证失败，参数错误！");
		}

		[HttpPost, Route("/api/wechat")]
		public IActionResult Post(PostModel model)
		{
			if (!appService.Check(app, model))
			{
				return Content("WeChatOpen服务验证失败，参数错误！");
			}

			int maxRecordCount = 10;
			string s = new StreamReader(Request.Body).ReadToEnd();

			var weOpenHandler = new WeChatHandler(new MemoryStream(Encoding.UTF8.GetBytes(s)), model, maxRecordCount);
			try
			{
				using (var stream = new FileStream(App.GetLocalPath("logs\\wechat\\req_" + DateTime.Now.ToStr("MMddHHmmss") + "_" + App.IdWorker.NextStringId() + "_" + weOpenHandler.RequestMessage.FromUserName + ".log"), FileMode.CreateNew, FileAccess.ReadWrite))
				{
					weOpenHandler.RequestDocument.Save((Stream)stream);
				}
				weOpenHandler.OmitRepeatedMessage = true;
				weOpenHandler.Execute();

				var path = App.GetLocalPath("logs\\wechat\\res_" + DateTime.Now.ToStr("MMddHHmmss") + "_" + App.IdWorker.NextStringId() + "_" + weOpenHandler.RequestMessage.FromUserName + ".log");
				if (weOpenHandler.ResponseDocument != null)
				{
					using (var stream2 = new FileStream(path, FileMode.CreateNew, FileAccess.ReadWrite))
					{
						weOpenHandler.ResponseDocument.Save((Stream)stream2);
					}
				}
				return new FixWeixinBugWeixinResult(weOpenHandler);
			}
			catch (Exception ex)
			{
				using (FileStream stream3 = new FileStream(App.GetLocalPath("logs\\wechat\\err_" + DateTime.Now.ToStr("MMddHHmmss") + "_" + App.IdWorker.NextStringId() + "_" + weOpenHandler.RequestMessage.FromUserName + ".log"), FileMode.CreateNew, FileAccess.ReadWrite))
				{
					using (StreamWriter streamWriter = new StreamWriter(stream3))
					{
						streamWriter.WriteLine("ExecptionMessage:" + ex.Message);
						streamWriter.WriteLine(ex.Source);
						streamWriter.WriteLine(ex.StackTrace);
						if (weOpenHandler.ResponseDocument != null)
						{
							streamWriter.WriteLine(weOpenHandler.ResponseDocument.ToString());
						}
						if (ex.InnerException != null)
						{
							streamWriter.WriteLine("========= InnerException =========");
							streamWriter.WriteLine(ex.InnerException.Message);
							streamWriter.WriteLine(ex.InnerException.Source);
							streamWriter.WriteLine(ex.InnerException.StackTrace);
						}
						streamWriter.Flush();
					}
				}
				return Content("");
			}
		}
        #endregion

        #region menu
        [HttpGet, MultiAuthorize]
		public IActionResult GetMenu()
		{
			try
			{
				return Ok(menuService.GetMenu(app));
			}
			catch (ErrorJsonResultException ex)
			{
				return Ok(ex.JsonResult);
			}
		}

		[HttpGet, MultiAuthorize]
		public IActionResult CreateMenu()
		{
			try
			{
				return Ok(menuService.CreateMenu(app));
			}
			catch (ErrorJsonResultException ex)
			{
				return Ok(ex.JsonResult);
			}
		}

		[HttpDelete, MultiAuthorize]
		public IActionResult DeleteMenu()
		{
			try
			{
				return Ok(menuService.DeleteMenu(app));
			}
			catch (ErrorJsonResultException ex)
			{
				return Ok(ex.JsonResult);
			}
		}
        #endregion

        #region media
        [HttpPost, MultiAuthorize]
		public IActionResult UploadMedia([FromBody] JObject data)
		{
			var mediaUrl = data.Value<string>("mediaids");
			return Ok(mediaService.UploadMedia(app, mediaUrl));
		}

		[HttpDelete, MultiAuthorize]
		public IActionResult DeleteMedia(string mediaIds)
		{
            mediaService.DeleteMedias(app, mediaIds);
			return Ok(null);
		}
        #endregion

        #region news
        [HttpPost, MultiAuthorize]
		public IActionResult UploadNews([FromBody] JObject data)
		{
			var newsIds = data.Value<string>("newsids");
			var value = data.GetValue("comment", 1);
			return Ok(newsService.UploadNews(app, newsIds, value));
		}

		[HttpPost, MultiAuthorize]
		public IActionResult PreviewMedia([FromBody] JObject data)
		{
			var mediaId = data.Value<long>("mediaid");
			var wxNames = data.Value<string>("wxname");
			mediaService.PreviewMedia(app, mediaId, wxNames);
			return Ok(null);
		}

		[HttpPost, MultiAuthorize]
		public IActionResult SendMedia([FromBody] JObject data)
		{
			var mediaId = data.Value<long>("mediaid");
			return Ok(mediaService.SendMedia(app, mediaId));
		}
        #endregion

        #region login
        [HttpPost]
		public IActionResult WxLogin([FromBody] JObject data)
		{
			SessionBag sessionBag = null;
            var sessionId = data.GetValue("sessionid", "");
            if (sessionId.HasValue())
			{
				sessionBag = SessionContainer.GetSession(sessionId);
			}

			string message = null;
			if (sessionBag == null)
			{
				var code = data.GetValue("code", "");
				var result = WeChatApi.WxLogin(app, code);
				if (result.ErrorCodeValue == 0)
				{
					sessionBag = SessionContainer.UpdateSession(null, result.openid, result.session_key, result.unionid);
				}
				else
				{
					message = result.errmsg;
				}
			}
			else
			{
				var obj = appCtx.Session.ExecuteScalar(
                    new RequestContext("wx", "exec_wx_login")
                    .SetParam(new
				    {
					    appid = app.Id,
					    authtype = "wechat",
					    authid = sessionBag.OpenId
				    })
                );
				if (obj == null)
				{
					var cryptedData = data.GetValue("encryptedData", "");
					var iv = data.GetValue("iv", "");
					var decodedUserInfo = EncryptHelper.DecodeUserInfoBySessionId(sessionBag.Key, cryptedData, iv);
					obj = appCtx.Session.ExecuteScalar(
                        new RequestContext("wx", "exec_wx_regist")
                        .SetParam(new
					    {
						    newid = App.IdWorker.NextId(),
						    nickname = decodedUserInfo.nickName,
						    photo = decodedUserInfo.avatarUrl,
						    province = decodedUserInfo.province,
						    city = decodedUserInfo.city,
						    country = decodedUserInfo.country,
						    appid = app.Id,
						    authtype = "wechat",
						    authid = decodedUserInfo.openId,
						    unionid = decodedUserInfo.unionId
					    })
                    );
				}
				return Json(new
				{
					success = true,
					token = $"{sessionBag.Key}",
					message = "ok"
				});
			}

			return Json(new
			{
				success = false,
				message
			});
		}

		[HttpGet]
		public IActionResult WxCheckSession(string sessionId)
		{
			if (SessionContainer.GetSession(sessionId) != null)
			{
				return Json(new
				{
					success = true,
					message = "ok"
				});
			}

			return Json(new
			{
				success = false,
				message = "会话已过期"
			});
		}
        #endregion
    }
}

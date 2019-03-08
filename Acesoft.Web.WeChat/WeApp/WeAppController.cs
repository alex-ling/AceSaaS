using System;
using System.IO;
using System.Text;

using Microsoft.AspNetCore.Mvc;
using Senparc.Weixin.MP.MvcExtension;
using Senparc.Weixin.WxOpen.Entities.Request;
using Senparc.Weixin.WxOpen.Containers;
using Senparc.Weixin.WxOpen.Helpers;
using Newtonsoft.Json.Linq;
using Acesoft.Data;
using Acesoft.Web.Controllers;
using Acesoft.Web.WeChat.Entity;
using Acesoft.Web.WeChat.WeApp;

namespace Acesoft.Web.WeChat
{
	[ApiExplorerSettings(GroupName = "plat")]
	[Route("api/[controller]/[action]")]
	public class WeAppController : ApiControllerBase
	{
        private IAppService appService;
        private Wx_App app;

        public WeAppController(IWeChatContainer container, IAppService appService)
		{
            this.app = container.GetApp();
            this.appService = appService;
        }

        #region api
        [HttpGet, Route("/api/weapp")]
		public IActionResult Get(PostModel model, string echostr)
		{
			if (appService.Check(app, model))
			{
				return Content(echostr);
			}
			return Content("WeChatApp服务验证失败，参数错误！");
		}

		[HttpPost, Route("/api/weapp")]
		public IActionResult Post(PostModel model)
		{
			if (!appService.Check(app, model))
			{
				return Content("WeChatApp服务验证失败，参数错误！");
			}

			int maxRecordCount = 10;
			var s = new StreamReader(Request.Body).ReadToEnd();
			var handler = new WeAppHandler(new MemoryStream(Encoding.UTF8.GetBytes(s)), model, maxRecordCount);
			try
			{
				using (var stream = new FileStream(App.GetLocalPath("logs\\wechat\\req_" + DateTime.Now.ToStr("MMddHHmmss") + "_" + App.IdWorker.NextStringId() + "_" + handler.RequestMessage.FromUserName + ".log"), FileMode.CreateNew, FileAccess.ReadWrite))
				{
					handler.RequestDocument.Save(stream);
				}
				handler.OmitRepeatedMessage = true;
				handler.Execute();

				var path = App.GetLocalPath("logs\\wechat\\res_" + DateTime.Now.ToStr("MMddHHmmss") + "_" + App.IdWorker.NextStringId() + "_" + handler.RequestMessage.FromUserName + ".log");
				if (handler.ResponseDocument != null)
				{
					using (var stream2 = new FileStream(path, FileMode.CreateNew, FileAccess.ReadWrite))
					{
						handler.ResponseDocument.Save((Stream)stream2);
					}
				}
				return new FixWeixinBugWeixinResult(handler);
			}
			catch (Exception ex)
			{
				using (var stream3 = new FileStream(App.GetLocalPath("logs\\wechat\\err_" + DateTime.Now.ToStr("MMddHHmmss") + "_" + App.IdWorker.NextStringId() + "_" + handler.RequestMessage.FromUserName + ".log"), FileMode.CreateNew, FileAccess.ReadWrite))
				{
					using (StreamWriter streamWriter = new StreamWriter(stream3))
					{
						streamWriter.WriteLine("ExecptionMessage:" + ex.Message);
						streamWriter.WriteLine(ex.Source);
						streamWriter.WriteLine(ex.StackTrace);
						if (handler.ResponseDocument != null)
						{
							streamWriter.WriteLine(handler.ResponseDocument.ToString());
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
                var obj = AppCtx.Session.ExecuteScalar(
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
                    obj = AppCtx.Session.ExecuteScalar(
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

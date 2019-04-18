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
using Microsoft.Extensions.Logging;

namespace Acesoft.Web.WeChat
{
	[ApiExplorerSettings(GroupName = "plat")]
	[Route("api/[controller]/[action]")]
	public class WeAppController : ApiControllerBase
    {
        private ILogger<WeAppController> logger;
        private IAppService appService;
        private Wx_App app;

        public WeAppController(ILogger<WeAppController> logger, 
            IWeChatContainer container, 
            IAppService appService)
		{
            this.logger = logger;
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

			var maxRecordCount = 10;
			var s = new StreamReader(Request.Body).ReadToEnd();
			var h = new WeAppHandler(new MemoryStream(Encoding.UTF8.GetBytes(s)), model, maxRecordCount);

			try
			{
                logger.LogDebug($"Receiving from {h.RequestMessage.FromUserName} with XML:\n{h.RequestDocument.Root}");

                h.OmitRepeatedMessage = true;
				h.Execute();

                logger.LogDebug($"Sending to {h.RequestMessage.FromUserName} with XML:\n{h.ResponseDocument.Root}");
                return new FixWeixinBugWeixinResult(h);
			}
			catch (Exception ex)
			{
                logger.LogWarning(ex.GetException(), $"Sending to {h.RequestMessage.FromUserName} with error");
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

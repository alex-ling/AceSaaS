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
using Acesoft.Rbac;
using System.Linq;

namespace Acesoft.Web.WeChat
{
	[ApiExplorerSettings(GroupName = "plat")]
	[Route("api/[controller]/[action]")]
	public class WeAppController : AuthController
    {
        private readonly ILogger<WeAppController> logger;
        private readonly IAppService appService;
        private readonly IWechatService wechatService;
        private Wx_App app;

        public WeAppController(
            ILogger<WeAppController> logger,
            IWeChatContainer container,
            IAppService appService,
            IWechatService wechatService)
            : base(logger)
        {
            this.logger = logger;
            this.app = container.GetApp();
            this.appService = appService;
            this.wechatService = wechatService;
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
        public IActionResult WxLogin([FromBody]JObject data)
        {
            var token = AppCtx.AC.TryWeappAutoLogin(data, out string errMsg);
            if (token != null)
            { 
                return Json(new
                {
                    success = true,
                    token,
                    message = "ok"
                });
            }

            return Json(new
            {
                success = false,
                errMsg
            });
        }

        [HttpGet]
        public IActionResult WxCheckSession(string token)
        {
            var key = token.Split('-').First();
            if (SessionContainer.GetSession(key) != null)
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

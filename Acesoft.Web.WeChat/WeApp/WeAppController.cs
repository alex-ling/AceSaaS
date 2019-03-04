using System;
using System.IO;
using System.Text;

using Acesoft.Web.WeChat.Entity;
using Acesoft.Web.WeChat.WeApp;
using Microsoft.AspNetCore.Mvc;
using Senparc.Weixin.MP.MvcExtension;
using Senparc.Weixin.WxOpen.Entities.Request;

namespace Acesoft.Web.WeChat
{
	[ApiExplorerSettings(GroupName = "plat")]
	[Route("api/[controller]/[action]")]
	public class WeAppController : Controller
	{
        private IApplicationContext appCtx;
        private IAppService appService;
        private Wx_App app;

        public WeAppController(
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
        }

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
	}
}

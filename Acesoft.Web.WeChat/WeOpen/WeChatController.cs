
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Senparc.Weixin.Exceptions;
using Senparc.Weixin.MP.Entities.Request;
using Senparc.Weixin.MP.MvcExtension;
using Newtonsoft.Json.Linq;
using Acesoft.Rbac;
using Acesoft.Web.Controllers;
using Acesoft.Web.WeChat.Entity;
using Acesoft.Web.WeChat.WeOpen;
using Acesoft.Web.Mvc;

namespace Acesoft.Web.WeChat
{
    [ApiExplorerSettings(GroupName = "plat")]
	[Route("api/[controller]/[action]")]
	public class WeChatController : AuthController
	{
        private readonly ILogger<WeChatController> logger;
        private readonly IAppService appService;
        private readonly IMenuService menuService;
        private readonly IMediaService mediaService;
        private readonly INewsService newsService;
        private readonly IActivityService activityService;
        private readonly IVoteService voteService;
        private Wx_App app;

		public WeChatController(
            ILogger<WeChatController> logger,
            IWeChatContainer container,
            IAppService appService,
            IMenuService menuService,
            IMediaService mediaService,
            INewsService newsService,
            IActivityService activityService,
            IVoteService voteService) 
            : base(logger)
		{
            this.logger = logger;
            this.app = container.GetApp();
			this.appService = appService;
            this.menuService = menuService;
            this.mediaService = mediaService;
            this.newsService = newsService;
            this.activityService = activityService;
            this.voteService = voteService;
		}

        #region external
        [HttpGet]
		public IActionResult GetToken()
		{
			var secret = App.GetQuery("secret", "");
            return Ok(appService.GetToken(app, secret));
        }

        [HttpPost]
        public async Task<IActionResult> WeLogin([FromBody]JObject data)
        {
            if (data.GetValue<string>("regtype").HasValue())
            {
                // 自动创建用户
                base.PostUser(data);
            }

            var userName = data.GetValue<string>("username");
            if (!userName.HasValue())
            {
                userName = data.GetValue<string>("mobile");
                if (!userName.HasValue())
                {
                    userName = data.GetValue<string>("mail");
                }
            }

            var password = data.GetValue<string>("password");
            var openId = data.GetValue<string>("openid");
            await AppCtx.AC.WechatLogin(userName, password, openId);

            return Ok(null);
        }
        #endregion

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

			var maxRecordCount = 10;
			var str = new StreamReader(Request.Body).ReadToEnd();
			var h = new WeChatHandler(HttpContext.RequestServices, app,
                new MemoryStream(Encoding.UTF8.GetBytes(str)), model, maxRecordCount);

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
		public IActionResult UploadMedia([FromBody]JObject data)
		{
			var mediaIds = data.Value<string>("mediaids");
            mediaService.UploadMedias(app, mediaIds);
            return Ok(null);
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
		public IActionResult UploadNews([FromBody]JObject data)
		{
			var newsIds = data.Value<string>("newsids");
			var value = data.GetValue("comment", 1);
			return Ok(newsService.UploadNews(app, newsIds, value));
		}

		[HttpPost, MultiAuthorize]
		public IActionResult PreviewMedia([FromBody]JObject data)
		{
			var mediaId = data.Value<long>("mediaid");
			var wxNames = data.Value<string>("wxname");
			mediaService.PreviewMedia(app, mediaId, wxNames);
			return Ok(null);
		}

		[HttpPost, MultiAuthorize]
		public IActionResult SendMedia([FromBody]JObject data)
		{
			var mediaId = data.Value<long>("mediaid");
			return Ok(mediaService.SendMedia(app, mediaId));
		}
        #endregion

        #region activity
        [HttpPost, MultiAuthorize]
        public IActionResult CreateQrCode([FromBody]JObject data)
        {
            var activityIds = data.Value<string>("activityids");
            activityService.CreateQrCode(app, activityIds);
            return Ok(null);
        }
        #endregion

        #region vote
        [HttpPost, Action("微信投票")]
        public IActionResult PostVote([FromBody]JObject data)
        {
            var voteItemId = data.GetValue<long>("voteitemid");
            var openId = data.GetValue<string>("openid");
            var content = data.GetValue<string>("content", "");
            var result = voteService.Vote(voteItemId, openId, content);
            return Ok(result);
        }
        #endregion
    }
}

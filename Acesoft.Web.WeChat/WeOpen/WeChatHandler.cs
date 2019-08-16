using System;
using System.IO;

using Microsoft.Extensions.DependencyInjection;
using Acesoft.Platform;
using Senparc.NeuChar.Entities;
using Senparc.Weixin.MP.Entities;
using Senparc.Weixin.MP.Entities.Request;
using Senparc.Weixin.MP.MessageHandlers;
using Acesoft.Web.WeChat.Entity;
using System.Linq;
using Acesoft.Util.Helper;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System.Web;

namespace Acesoft.Web.WeChat.WeOpen
{
	public class WeChatHandler : MessageHandler<WeChatContext>
	{
        private readonly IServiceProvider services;
        private readonly IConfigService configService;
        private readonly IActivityService activityService;
        private readonly Wx_App app;
        private readonly ILogger logger;

        public WeChatHandler(IServiceProvider services, Wx_App app,
            Stream inputStream, PostModel postModel, int maxRecordCount)
			: base(inputStream, postModel, maxRecordCount)
		{
            this.services = services;
            this.app = app;
            this.configService = services.GetService<IConfigService>();
            this.activityService = services.GetService<IActivityService>();
            this.logger = services.GetService<ILogger<WeChatHandler>>();
		}

		public override void OnExecuting()
		{
			base.OnExecuting();
		}

		public override void OnExecuted()
		{
			base.OnExecuted();
		}

        public override IResponseMessageBase OnTextOrEventRequest(RequestMessageText requestMessage)
        {
            var word = requestMessage.Content;
            if (word == "客服" || word == "人工咨询" || word == "customer_service")
            {
                return CreateResponseMessage<ResponseMessageTransfer_Customer_Service>();
            }
            return null;
        }

        public override IResponseMessageBase DefaultResponseMessage(IRequestMessageBase requestMessage)
        {
            return new SuccessResponseMessage();
        }

        public override IResponseMessageBase OnEvent_SubscribeRequest(RequestMessageEvent_Subscribe requestMessage)
        {
            if (requestMessage.EventKey != null && requestMessage.EventKey.StartsWith("qrscene_"))
            {
                DoCustomAction(requestMessage.FromUserName, requestMessage.EventKey.Substring(8));
            }

            var sysCfg = configService.GetConfig(app.Id);
            var res = CreateResponseMessage<ResponseMessageText>();
            res.Content = sysCfg.GetValue("subscribe", "欢迎您关注，更多信息请查看菜单！");
            return res;
        }

        public override IResponseMessageBase OnEvent_ScanRequest(RequestMessageEvent_Scan requestMessage)
        {
            if (requestMessage.EventKey != null)
            {
                DoCustomAction(requestMessage.FromUserName, requestMessage.EventKey);
            }
            return base.OnEvent_ScanRequest(requestMessage);
        }

        public override IResponseMessageBase OnEvent_ClickRequest(RequestMessageEvent_Click requestMessage)
        {
            if (requestMessage.EventKey != null)
            {
                var cmd = requestMessage.EventKey.Split('?').First();
                switch (cmd)
                {
                    default:
                        break;
                }
            }
            return base.OnEvent_ClickRequest(requestMessage);
        }

        // 活动通过客服接口发送消息
        private void DoCustomAction(string openId, string activityId)
        {
            var activity = activityService.Get(long.Parse(activityId));
            activity.Poster = App.GetWebPath(activity.Poster, true);

            // 设置自动登录
            var url = $"/plat/account/login?appid={app.Id}&returnUrl={HttpUtility.UrlEncode(activity.Url)}";
            activity.Url = App.GetWebPath(url, true);
            logger.LogDebug($"Wechat going to Activity URL:{activity.Url}");

            Task.Run(() =>
            {
                System.Threading.Thread.Sleep(100);
                try
                {
                    logger.LogDebug($"Sending ActivityId:{activityId} to OpenId:{openId}");
                    activityService.SendActivity(app, activity, openId);
                    logger.LogDebug($"Sended ActivityId:{activityId} to OpenId:{openId}");
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, $"Sending activity with error!");
                }
            });
        }
    }
}

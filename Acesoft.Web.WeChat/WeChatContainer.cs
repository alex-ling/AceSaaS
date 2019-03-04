using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Acesoft.Logger;
using Acesoft.Web.WeChat.Entity;
using Senparc.Weixin.Containers;
using Senparc.Weixin.MP.Containers;
using System.Collections.Concurrent;

namespace Acesoft.Web.WeChat
{
	public class WeChatContainer : IWeChatContainer
	{
        private readonly ILogger logger = LoggerContext.GetLogger<WeChatContainer>();
		private readonly ConcurrentDictionary<long, Wx_App> apps = new ConcurrentDictionary<long, Wx_App>();
        private readonly IAppService appService;

        public WeChatContainer(IAppService appService)
        {
            this.appService = appService;
        }

		public Wx_App GetApp(long appId)
		{
			return apps.GetOrAdd(appId, key =>
			{
				var wxApp = appService.Get(appId);
				if (wxApp == null)
				{
					throw new AceException("传入的参数[appid]不存在！");
				}

				if (!BaseContainer<AccessTokenBag>.CheckRegistered(wxApp.AppId))
				{
					AccessTokenContainer.Register(wxApp.AppId, wxApp.AppSecret);
				}

				logger.LogDebug($"Initalize WeChatApp for id: {appId}");
				return wxApp;
			});
		}

		public Wx_App GetApp()
		{
			var query = App.GetQuery<long>("appid");
			logger.LogDebug($"Get WeChatApp for id: [{query}]");
			return GetApp(query);
		}
	}
}

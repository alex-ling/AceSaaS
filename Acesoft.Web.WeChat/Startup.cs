using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Hosting;
using Acesoft.Web.WeChat.Services;
using Senparc.CO2NET.RegisterServices;
using Senparc.Weixin.RegisterServices;
using Acesoft.Config;
using Microsoft.Extensions.Options;
using Senparc.CO2NET;
using Senparc.CO2NET.Cache;
using Senparc.Weixin;
using Senparc.Weixin.Entities;

namespace Acesoft.Web.WeChat
{
    public class Startup : StartupBase
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            // regist
            services.AddSingleton<IWeChatContainer, WeChatContainer>();

            // regist services.
            services.AddSingleton<IAppService, AppService>();
            services.AddSingleton<IMediaService, MediaService>();
            services.AddSingleton<IMenuService, MenuService>();
            services.AddSingleton<INewsService, NewsService>();

            // wechat config.
            var configuration = ConfigContext.GetJsonConfig(opts =>
            {
                opts.Optional = true;
                opts.ConfigFile = "wechat.config.json";
            });

            // Senparc.CO2NET
            services.AddSenparcGlobalServices(configuration);

            // Senparc.Weixin
            services.AddSenparcWeixinServices(configuration);
        }

        public override void Configure(IApplicationBuilder app, IRouteBuilder routes, IServiceProvider services)
        {
            var env = services.GetService<IHostingEnvironment>();
            var senparcSetting = services.GetService<IOptions<SenparcSetting>>().Value;
            var senparcWeixinSetting = services.GetService<IOptions<SenparcWeixinSetting>>().Value;

            // wechat
            var register = RegisterService.Start(env, senparcSetting).UseSenparcGlobal();
            register.UseSenparcWeixin(senparcWeixinSetting, senparcSetting);
        }
    }
}
   
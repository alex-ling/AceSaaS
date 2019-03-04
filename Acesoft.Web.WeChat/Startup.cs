using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Acesoft.Web.WeChat.Services;

namespace Acesoft.Web.WeChat
{
    public class Startup : StartupBase
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            // regist
            services.AddSingleton<IAppService, AppService>();
            services.AddSingleton<IMediaService, MediaService>();
            services.AddSingleton<IMenuService, MenuService>();
            services.AddSingleton<INewsService, NewsService>();
            services.AddSingleton<IWeChatContainer, WeChatContainer>();
        }

        public override void Configure(IApplicationBuilder app, IRouteBuilder routes, IServiceProvider services)
        {

        }
    }
}
   
using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Acesoft.Web.DataAccess;
using Acesoft.Web.Portal;
using Acesoft.Web.Portal.Services;

namespace Acesoft.Web.Portal
{
    public class Startup : StartupBase
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            // regist portal services.
            services.AddSingleton<IWidgetService, WidgetService>();
            services.AddSingleton<IPageService, PageService>();
            services.AddSingleton<IModuleService, ModuleService>();
        }

        public override void Configure(IApplicationBuilder app, IRouteBuilder routes, IServiceProvider services)
        {
            //routes.MapRoute()
        }
    }
}

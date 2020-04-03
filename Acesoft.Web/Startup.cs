using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Acesoft.Web.DataAccess;

namespace Acesoft.Web
{
    public class Startup : StartupBase
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            // add app global config. WebHostBuilder extension to add.
            //services.AddAppConfig();

            // add data access for every Tenant.
            services.AddDataAccess();

            // regist application context.
            services.AddSingleton<IApplicationContext, ApplicationContext>();
        }

        public override void Configure(IApplicationBuilder app, IRouteBuilder routes, IServiceProvider services)
        {
            //routes.MapRoute()
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

using Acesoft.Core;
using Acesoft.Data;
using Acesoft.Web.Data;
using Acesoft.Web.StateProviders;

namespace Acesoft.Web
{
    public class Startup : StartupBase
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddDataAccess();

            services.AddSingleton<IApplicationStateProvider, UserStateProvider>();
            services.AddSingleton<IApplicationContext, ApplicationContext>();
            //services.AddScoped<IApplicationContext, ApplicationContext>();
        }

        public override void Configure(IApplicationBuilder app, IRouteBuilder routes, IServiceProvider services)
        {
        }
    }
}

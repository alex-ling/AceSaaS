using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Acesoft.Config;
using Acesoft.Web.Cloud.Config;

namespace Acesoft.Web.Cloud
{
    public class Startup : StartupBase
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            // add config.
            services.AddJsonConfig<CloudConfig>(opts =>
            {
                opts.ConfigFile = "cloud.config.json";
                opts.IsTenantConfig = true;
            });

            // add cloud service.
            services.AddSingleton<ICloudService, CloudService>();
        }

        public override void Configure(IApplicationBuilder app, IRouteBuilder routes, IServiceProvider services)
        {
        }
    }
}

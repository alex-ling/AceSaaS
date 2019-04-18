using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Acesoft.Web.IoT.Client;
using Acesoft.Web.IoT.WsClient;
using Acesoft.Web.IoT.Services;
using Acesoft.Web.IoT.Config;
using Acesoft.Config;
using Acesoft.Web.IoT.Hubs;

namespace Acesoft.Web.IoT
{
    public class Startup : StartupBase
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            // add iot config.
            services.AddJsonConfig<IotConfig>(opts =>
            {
                opts.ConfigFile = "iot.config.json";
                opts.IsTenantConfig = true;
            });

            // add iot client service.
            services.AddSingleton<IIotClient, IotClient>();

            // add websocket client service.
            services.AddSingleton<IIotWsClient, IotWsClient>();

            // add services.
            services.AddSingleton<ICacheService, CacheService>();
            services.AddSingleton<IIotService, IotService>();

            // add signalR
            services.AddSignalR();
        }

        public override void Configure(IApplicationBuilder app, IRouteBuilder routes, IServiceProvider services)
        {
            app.UseSignalR(route =>
            {
                route.MapHub<IotDataHub>("/hubs/iotdata");
                route.MapHub<IotServiceHub>("/hubs/iotservice");
            });
        }
    }
}

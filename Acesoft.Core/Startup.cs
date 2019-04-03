using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Acesoft.Cache;
using Acesoft.Util;
using Acesoft.Config;
using Acesoft.Logger;
using Acesoft.Security;

namespace Acesoft.Core
{
    public class Startup : StartupBase
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            // add cache
            services.AddMemoryCache();
            services.AddDistributedRedisCache();

            // add util
            services.AddSingleton<FileWatcher>();
            services.AddSingleton<IByteCrypto, SwapByteCrypto>();
            //services.AddScoped<IRSACrypto, RSACrypto>();

            // add logging
            services.AddLogging();
        }

        public override void Configure(IApplicationBuilder app, IRouteBuilder routes, IServiceProvider services)
        {
            // set global app
            services.UseAppContext();

            // use logger
            services.UseLoggerContext();

            // use config
            services.UseConfigContext();
        }
    }
}

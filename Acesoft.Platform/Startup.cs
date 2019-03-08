using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Acesoft.Data;
using Acesoft.Platform.Services;
using Acesoft.Platform.Schema;

namespace Acesoft.Platform
{
    public class Startup : StartupBase
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            // regist schema
            services.AddSingleton<IStoreSchema, PlatformSchema>();
            services.AddSingleton<IStoreSchema, AppSchema>();

            // regist services
            services.AddSingleton<IConfigService, ConfigService>();
            services.AddSingleton<ITableService, TableService>();
            services.AddSingleton<IFieldService, FieldService>();
        }

        public override void Configure(IApplicationBuilder app, IRouteBuilder routes, IServiceProvider services)
        {

        }
    }
}
   
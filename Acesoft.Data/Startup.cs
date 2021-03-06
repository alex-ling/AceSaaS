﻿using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

using Acesoft.Core;
using Acesoft.Data.Sql;

namespace Acesoft.Data
{
    public class Startup : StartupBase
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            // 初始化全局Id实例
            services.AddSingleton<ISchemaCreator, SchemaCreator>();
            services.AddSingleton<IIdWorker>(new IdWorker(0, 0));

            // 初始化数据配置
            services.AddDataConfig();
        }

        public override void Configure(IApplicationBuilder app, IRouteBuilder routes, IServiceProvider services)
        {
        }
    }
}

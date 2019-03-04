using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace Acesoft.Cache
{
    public class Startup : StartupBase
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            // 添加内存缓存
            services.AddMemoryCache();

            // 添加Redis分布式缓存
            services.AddDistributedRedisCache();
        }

        public override void Configure(IApplicationBuilder app, IRouteBuilder routes, IServiceProvider services)
        {
            services.UseCacheContext();
        }
    }
}

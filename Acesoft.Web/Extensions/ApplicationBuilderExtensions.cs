using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Routing;

using Acesoft.Util;
using Acesoft.Web.Multitenancy;
using Acesoft.Web.Modules;

namespace Acesoft.Web
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseMultitenancy(this IApplicationBuilder app)
        {
            var services = app.ApplicationServices;
            var env = services.GetRequiredService<IHostingEnvironment>();
            var routes = new RouteBuilder(app);

            // 加载modules
            services.GetService<ModuleContainer>().Configure(app, routes, services);

            // 注册multitenant中间件
            app.UseMiddleware<TenantResolutionMiddleware<Tenant>>();

            return app;
        }

        public static IApplicationBuilder UsePerTenant(this IApplicationBuilder app,       
            Action<TenantPipelineBuilderContext<Tenant>, IApplicationBuilder> configure)
        {
            // 按每个tenant进行注册、配置
            return app.Use(next => new TenantPipelineMiddleware<Tenant>(next, app, configure).Invoke);
        }
    }
}

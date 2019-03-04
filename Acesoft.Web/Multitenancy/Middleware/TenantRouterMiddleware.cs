using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Builder.Internal;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Internal;
using Microsoft.AspNetCore.Routing;

namespace Acesoft.Web.Multitenancy
{
    public class TenantRouterMiddleware
    {
        private readonly RequestDelegate next;
        //private readonly Action<TenantContext, IRouteBuilder> configure;
        private readonly ConcurrentDictionary<string, Lazy<RequestDelegate>> pipelines
            = new ConcurrentDictionary<string, Lazy<RequestDelegate>>();

        public TenantRouterMiddleware(
            RequestDelegate next)
        {
            this.next = next;
            //this.configure = configure;
        }

        public async Task Invoke(HttpContext context)
        {
            var tenantContext = context.GetTenantContext();
            if (tenantContext != null)
            {
                var tenantPipeline = pipelines.GetOrAdd(
                    tenantContext.Tenant.Name,
                    new Lazy<RequestDelegate>(() => BuildTenantPipeline(tenantContext)));

                await tenantPipeline.Value(context);
            }
        }

        private RequestDelegate BuildTenantPipeline(TenantContext tenantContext)
        {
            var tenantServices = tenantContext.ServiceProvider;
            var appBuilder = new ApplicationBuilder(tenantServices);
            var routeBuilder = new RouteBuilder(appBuilder)
            {
                DefaultHandler = tenantServices.GetRequiredService<MvcRouteHandler>()
            };

            var startups = tenantServices.GetServices<IStartup>();

            // IStartup instances are ordered by module dependency with an Order of 0 by default.
            // OrderBy performs a stable sort so order is preserved among equal Order values.
            startups.OrderBy(s => s.Order).Each(s => s.Configure(appBuilder, routeBuilder, tenantServices));

            // use Tenant router
            var router = routeBuilder.Build();
            appBuilder.UseRouter(router);

            // register root pipeline at the end of the tenant branch
            appBuilder.Run(next);

            return appBuilder.Build();
        }
    }
}

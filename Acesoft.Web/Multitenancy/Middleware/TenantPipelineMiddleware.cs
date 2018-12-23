using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace Acesoft.Web.Multitenancy
{
    public class TenantPipelineMiddleware
    {
        private readonly RequestDelegate next;
        private readonly IApplicationBuilder rootApp;
        private readonly Action<TenantContext, IApplicationBuilder> configure;
        private readonly ConcurrentDictionary<string, Lazy<RequestDelegate>> pipelines
            = new ConcurrentDictionary<string, Lazy<RequestDelegate>>();

        public TenantPipelineMiddleware(
            RequestDelegate next,
            IApplicationBuilder rootApp,
            Action<TenantContext, IApplicationBuilder> configure)
        {
            this.next = next;
            this.rootApp = rootApp;
            this.configure = configure;
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
            var tenantBuilder = rootApp.New();

            configure(tenantContext, tenantBuilder);

            // register root pipeline at the end of the tenant branch
            tenantBuilder.Run(next);

            return tenantBuilder.Build();
        }
    }
}

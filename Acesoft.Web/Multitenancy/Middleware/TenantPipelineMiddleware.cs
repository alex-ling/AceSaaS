using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace Acesoft.Web.Multitenancy
{
    public class TenantPipelineMiddleware<T>
    {
        private readonly RequestDelegate next;
        private readonly IApplicationBuilder rootApp;
        private readonly Action<TenantPipelineBuilderContext<T>, IApplicationBuilder> configure;
        private readonly ConcurrentDictionary<T, Lazy<RequestDelegate>> pipelines
            = new ConcurrentDictionary<T, Lazy<RequestDelegate>>();

        public TenantPipelineMiddleware(
            RequestDelegate next,
            IApplicationBuilder rootApp,
            Action<TenantPipelineBuilderContext<T>, IApplicationBuilder> configure)
        {
            this.next = next;
            this.rootApp = rootApp;
            this.configure = configure;
        }

        public async Task Invoke(HttpContext context)
        {
            Check.Require(context != null, $"{nameof(context)} must not null");

            var tenantContext = context.GetTenantContext<T>();
            if (tenantContext != null)
            {
                var tenantPipeline = pipelines.GetOrAdd(
                    tenantContext.Tenant,
                    new Lazy<RequestDelegate>(() => BuildTenantPipeline(tenantContext)));

                await tenantPipeline.Value(context);
            }
        }

        private RequestDelegate BuildTenantPipeline(TenantContext<T> tenantContext)
        {
            var tenantBuilder = rootApp.New();
            var builderContext = new TenantPipelineBuilderContext<T>
            {
                TenantContext = tenantContext,
                Tenant = tenantContext.Tenant
            };

            configure(builderContext, tenantBuilder);

            // register root pipeline at the end of the tenant branch
            tenantBuilder.Run(next);

            return tenantBuilder.Build();
        }
    }
}

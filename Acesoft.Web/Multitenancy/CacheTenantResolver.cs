using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Acesoft.Web.Multitenancy
{
    public class CacheTenantResolver : MemoryTenantResolver<Tenant>
    {
        public CacheTenantResolver(IMemoryCache cache, 
            IOptions<TenantsConfig> tenants, ILogger<CacheTenantResolver> logger)
            : base(cache, tenants.Value, logger)
        {
        }

        protected override string GetContextIdentifier(HttpContext context)
        {
            return context.Request.Host.Value.ToLower();
        }

        protected override IEnumerable<string> GetTenantIdentifiers(TenantContext<Tenant> context)
        {
            return context.Tenant.Hostnames;
        }

        protected override Task<TenantContext<Tenant>> ResolveAsync(HttpContext context)
        {
            TenantContext<Tenant> tenantContext = null;

            // 按hostname进程匹配tenant
            var tenant = TenantsConfig.Tenants.FirstOrDefault(t =>
                t.Hostnames.Any(h => 
                    h.Equals(context.Request.Host.Value.ToLower())
                ));
            if (tenant == null)
            {
                tenant = TenantsConfig.Tenants.FirstOrDefault(t => t.Name == TenantsConfig.DefaultTenant);
            }
            if (tenant != null)
            {
                tenantContext = new TenantContext<Tenant>(tenant);
            }

            return Task.FromResult(tenantContext);
        }
    }
}

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
    /// <summary>
    /// Host+Prefix match
    /// </summary>
    public class DefaultTenantResolver : MemoryTenantResolver
    {
        private readonly TenantsConfig config;

        public DefaultTenantResolver(IMemoryCache cache, 
            IOptions<TenantsConfig> tenantsOption, 
            ILogger<DefaultTenantResolver> logger) : base(cache, logger)
        {
            this.config = tenantsOption.Value;
        }

        protected override string GetContextIdentifier(HttpContext context)
        {
            return context.Request.Host.Value.ToLower();
        }

        protected override IEnumerable<string> GetTenantIdentifiers(Tenant tenant)
        {
            return tenant.Hostnames;
        }

        protected override Task<Tenant> ResolveAsync(HttpContext context)
        {
            Tenant tenant = null;

            // match hostname
            var requestKey = context.Request.Host.Value.ToLower();
            tenant = config.Tenants.FirstOrDefault(t => t.Hostnames.Any(h => h.Equals(requestKey)));

            if (tenant == null)
            {
                tenant = config.Tenants.FirstOrDefault(t => t.Name.Equals(config.DefaultTenant));

                if (tenant == null)
                {
                    logger.LogDebug($"Resolved tenant failed. Check config with default tenant value.");
                }
                else
                {
                    logger.LogDebug($"Resolved tenant \"{tenant.Name}\" with default tenant.");
                }
            }
            else
            {
                logger.LogDebug($"Resolved Tenant \"{tenant.Name}\" with request: {requestKey}.");
            }

            return Task.FromResult(tenant);
        }
    }
}

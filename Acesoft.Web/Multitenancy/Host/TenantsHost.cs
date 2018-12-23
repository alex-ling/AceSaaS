using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Acesoft.Web.Multitenancy
{
    public class TenantsHost : ITenantsHost
    {
        private readonly ILogger logger;
        private readonly static object syncLock = new object();
        private readonly TenantsConfig tenantsConfig;
        private readonly ITenantContainerFactory tenantContainerFactory;

        private ConcurrentDictionary<string, TenantContext> contexts;

        public TenantsHost(IOptions<TenantsConfig> tenantsOption,
            ITenantContainerFactory factory,
            ILogger<TenantsHost> logger)
        {
            tenantsConfig = tenantsOption.Value;
            tenantContainerFactory = factory;
            this.logger = logger;
        }

        public void Initialize()
        {
            if (contexts == null)
            {
                lock (this)
                {
                    if (contexts == null)
                    {
                        logger.LogDebug("Starting initialize TenantsHost from config.");

                        contexts = new ConcurrentDictionary<string, TenantContext>();
                        CreateAndLoadTenants();
                    }
                }
            }
        }

        public TenantContext GetOrCreateContext(Tenant tenant)
        {
            return contexts.GetOrAdd(tenant.Name, _ =>
            {
                var provider = tenantContainerFactory.CreateContainer(tenant);
                return new TenantContext(tenant, provider);
            });
        }

        public void ReloadContext(Tenant tenant)
        {
            if (contexts.TryRemove(tenant.Name, out TenantContext context))
            {
                context.Dispose();
            }

            GetOrCreateContext(tenant);
        }

        void CreateAndLoadTenants()
        {
            var tenants = tenantsConfig.Tenants;
            
            // Load all tenants, and activate their shell.
            Parallel.ForEach(tenants, tenant =>
            {
                try
                {
                    GetOrCreateContext(tenant);
                    logger.LogDebug($"Load Tenant with name \"{tenant.Name}\" successful.");

                }
                catch (Exception ex)
                {
                    logger.LogError(ex, $"The Tenant \" {tenant.Name}\" could not be started.");

                    throw;
                }
            });

            logger.LogDebug("Done initialize tentants.");
        }
    }
}

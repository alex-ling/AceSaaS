using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Acesoft.Web.Modules;
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
        private readonly IModulesHost modulesHost;

        private ConcurrentDictionary<string, TenantContext> contexts;

        public TenantsHost(IOptions<TenantsConfig> tenantsOption,
            ITenantContainerFactory factory,
            IModulesHost modulesHost,
            ILogger<TenantsHost> logger)
        {
            tenantsConfig = tenantsOption.Value;
            tenantContainerFactory = factory;
            this.modulesHost = modulesHost;
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
                        this.modulesHost.Initialize();

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

        public TenantContext GetContext(string tenant)
        {
            return contexts[tenant];
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Acesoft.Web.Modules;

namespace Acesoft.Web.Multitenancy
{
    public class TenantContainerFactory : ITenantContainerFactory
    {
        private readonly IServiceCollection applicationServices;
        private readonly IServiceProvider serviceProvider;
        private readonly IModulesHost modulesHost;
        private readonly ILogger logger;

        public TenantContainerFactory(
            IServiceCollection applicationServices,
            IServiceProvider serviceProvider,
            IModulesHost modulesHost,
            ILogger<TenantContainerFactory> logger)
        {
            this.applicationServices = applicationServices;
            this.serviceProvider = serviceProvider;
            this.modulesHost = modulesHost;
            this.logger = logger;
        }

        public IServiceProvider CreateContainer(Tenant tenant)
        {
            var tenantServices = serviceProvider.CreateChildContainer(applicationServices);
            var startups = new List<IStartup>();

            // Add Acesoft.Web IStartup to first
            var mvcStartup = new Startup();
            startups.Add(mvcStartup);
            tenantServices.AddSingleton(mvcStartup);

            // Execute external module's IStartup
            foreach (var moduleName in tenant.Modules)
            {
                if (modulesHost.Modules.TryGetValue(moduleName, out ModuleWarpper module))
                {
                    // add IStartup
                    startups.Add(module.Startup);
                    tenantServices.AddSingleton(module.Startup);
                }
            }

            // configure services
            //var authenticationBuilder = 
            startups.OrderBy(s => s.Order).Each(s => s.SetTenant(tenant).ConfigureServices(tenantServices));

            // build
            return tenantServices.BuildServiceProvider(true);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using Acesoft.Core;
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

            this.modulesHost.Initialize();
        }

        public IServiceProvider CreateContainer(Tenant tenant)
        {
            var tenantServices = serviceProvider.CreateChildContainer(applicationServices);

            // Execute external module's IStartup registrations
            foreach (var moduleName in tenant.Modules)
            {
                if (modulesHost.Modules.TryGetValue(moduleName, out ModuleWarpper module))
                {
                    // add IStartup
                    tenantServices.AddSingleton(typeof(IStartup), module.StartupType);
                }
            }

            // build
            var tenantProvider = tenantServices.BuildServiceProvider(true);

            // configuare startups.
            var startups = tenantProvider.GetServices<IStartup>();
            startups.OrderBy(s => s.Order).Each(s => s.ConfigureServices(tenantServices));

            return tenantProvider;
        }
    }
}

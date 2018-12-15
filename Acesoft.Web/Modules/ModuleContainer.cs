using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;

using Acesoft.Core;
using Acesoft.Config;
using Acesoft.Util;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace Acesoft.Web.Modules
{
    public class ModuleContainer
    {
        private readonly ILogger logger = null;

        public IList<IStartup> Startups { get; } = new List<IStartup>();
        public IDictionary<string, ModuleConfig> Modules { get; } = new Dictionary<string, ModuleConfig>();

        public ModuleContainer(/*ILogger<ModuleContainer> logger*/)
        {
            //this.logger = logger;
        }

        public ModuleContainer ConfigureServices(IServiceCollection services)
        {
            // Add startup by order
            Startups.OrderBy(s => s.Order).Each(startup =>
            {
                startup.ConfigureServices(services);
            });

            return this;
        }

        public ModuleContainer Configure(IApplicationBuilder app, IRouteBuilder routes, IServiceProvider services)
        {
            // Configure startup by order
            Startups.OrderBy(s => s.Order).Each(startup =>
            {
                startup.Configure(app, routes, services);
            });

            return this;
        }

        public ModuleContainer LoadInternalModules()
        {
            // load modules in references
            Startups.Add(new Acesoft.Logger.Startup());
            Startups.Add(new Acesoft.Config.Startup());
            Startups.Add(new Acesoft.Cache.Startup());
            Startups.Add(new Acesoft.Web.Startup());

            return this;
        }

        public ModuleContainer LoadExternalModules()
        {
            var root = Path.Combine(AppContext.BaseDirectory, "Modules");
            logger?.LogDebug("Begin loading modules from {root}.", root);

            foreach (var moduleFolder in Directory.GetDirectories(root))
            {
                var module = ConfigContext.GetJsonConfig<ModuleConfig>(opts =>
                {
                    opts.ConfigFile = "module.config.json";
                    opts.ConfigPath = moduleFolder;
                });
                Modules.Add(module.Name, module);

                var assembly = Assembly.LoadFrom(Path.Combine(moduleFolder, module.MainAssembly));
                foreach (var type in assembly.GetTypes()
                    .Where(t => typeof(IStartup).IsAssignableFrom(t) && !t.IsAbstract))
                {
                    Startups.Add((IStartup)Dynamic.GetInstanceCreator(type)());
                    logger?.LogDebug("Found module: {typeName}.", type.Name);
                }
            }

            return this;
        }
    }
}

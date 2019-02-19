using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Routing;

using Acesoft.Core;
using Acesoft.Config;
using Acesoft.Util;

namespace Acesoft.Web.Modules
{
    public class ModulesHost : IModulesHost
    {
        private readonly ILogger logger;
        private readonly IMvcBuilder mvcBuilder;

        public IDictionary<string, ModuleWarpper> Modules { get; } = new Dictionary<string, ModuleWarpper>();

        public ModulesHost(IMvcBuilder mvcBuilder, ILogger<ModulesHost> logger)
        {
            this.mvcBuilder = mvcBuilder;
            this.logger = logger;
        }

        public void Initialize()
        {
            var root = Path.Combine(AppContext.BaseDirectory, "Modules");
            logger.LogDebug($"Start loading modules from {root}.");

            foreach (var moduleFolder in Directory.GetDirectories(root))
            {
                var module = ConfigContext.GetJsonConfig<ModuleConfig>(opts =>
                {
                    opts.ConfigFile = "module.config.json";
                    opts.ConfigPath = moduleFolder;
                });

                var assembly = Assembly.LoadFrom(Path.Combine(moduleFolder, module.MainAssembly));
                var startupType = assembly.GetTypes().FirstOrDefault(t => typeof(IStartup).IsAssignableFrom(t));
                var startup = (IStartup)Dynamic.GetInstanceCreator(startupType)();

                // this simple add/replace module.
                Modules[module.Name] = new ModuleWarpper(module, startup);
                logger.LogDebug($"Found module {module.Name} with type: {startupType.FullName}.");
                
                var partFactory = ApplicationPartFactory.GetApplicationPartFactory(assembly);
                foreach (var part in partFactory.GetApplicationParts(assembly))
                {
                    logger?.LogDebug($"Found mvc application part: {part.Name}.");
                    mvcBuilder.PartManager.ApplicationParts.Add(part);
                }
            }
        }
    }
}

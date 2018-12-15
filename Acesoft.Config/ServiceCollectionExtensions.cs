using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Acesoft.Config
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddJsonConfig<T>(this IServiceCollection services, Action<ConfigOption> options) where T : class
        {
            var option = new ConfigOption();
            options(option);

            var configuration = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), option.ConfigPath))
                .AddJsonFile(option.ConfigFile, optional: false, reloadOnChange: true)
                .Build();

            if (option.TenantConfig)
            {
                foreach (var section in configuration.GetChildren())
                {
                    services.Configure<T>(section.Key, section, option.Binder);
                }
            }
            else
            {
                services.Configure<T>(option.Name, configuration, option.Binder);
            }
            return services;
        }

        public static IServiceCollection AddXmlConfig<T>(this IServiceCollection services, Action<ConfigOption> options) where T : class
        {
            var option = new ConfigOption();
            options(option);

            var configuration = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), option.ConfigPath))
                .AddXmlFile(option.ConfigFile, optional: false, reloadOnChange: true)
                .Build();

            return services.Configure<T>(option.Name, configuration, option.Binder);
        }
    }
}

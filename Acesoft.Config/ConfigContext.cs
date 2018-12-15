using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Acesoft.Config
{
    public static class ConfigContext
    {
        static IServiceProvider _serviceProvider;
        static readonly ConcurrentDictionary<string, bool> _events = new ConcurrentDictionary<string, bool>();

        public static T GetJsonConfig<T>(Action<ConfigOption> options) where T : class, new()
        {
            var opts = new ConfigOption();
            options(opts);

            var basePath = opts.ConfigPath;
            if (!Path.IsPathRooted(basePath))
            {
                basePath = Path.Combine(Directory.GetCurrentDirectory(), basePath);
            }

            var configuration = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile(opts.ConfigFile, optional: opts.Optional, reloadOnChange: true)
                .Build();

            var config = new T();
            configuration.Bind(config);
            return config;
        }

        public static T GetConfig<T>(string nameOrTenant = "", Action<T, string> changed = null) where T : class, new()
        {
            if (changed == null)
            {
                var options = _serviceProvider.GetRequiredService<IOptionsSnapshot<T>>();
                return options.Get(nameOrTenant);
            }
            else
            {
                var options = _serviceProvider.GetRequiredService<IOptionsMonitor<T>>();
                options.OnChange((config, key) => 
                {
                    if (key == nameOrTenant)
                    {
                        Task.Run(() =>
                        {
                            _events.GetOrAdd(key, (_) =>
                            {
                                changed(config, key);
                                return true;
                            });

                            if (_events.ContainsKey(key) && _events[key])
                            {
                                _events[key] = false;

                                Thread.Sleep(500);
                                _events.TryRemove(key, out bool b);
                            }
                        });
                    }
                });
                return options.Get(nameOrTenant);
            }
        }

        public static IServiceProvider UseConfigContext(this IServiceProvider serviceProvider)
        {
            return _serviceProvider = serviceProvider;
        }
    }
}

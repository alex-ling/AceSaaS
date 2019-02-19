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
using Microsoft.Extensions.Logging;
using Acesoft.Config.Xml;
using Acesoft.Logger;
using Acesoft.Util;
using System.Xml;
using Acesoft.Core;

namespace Acesoft.Config
{
    public static class ConfigContext
    {
        static IServiceProvider _serviceProvider;
        static readonly ConcurrentDictionary<string, bool> _events = new ConcurrentDictionary<string, bool>();
        static readonly ILogger logger = LoggerContext.GetLogger(nameof(ConfigContext));
        static FileWatcher _watcher = null;

        #region json
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
                var options = (App.Context?.RequestServices ?? _serviceProvider).GetService<IOptionsSnapshot<T>>();
                return options.Get(nameOrTenant);
            }
            else
            {
                var options = (App.Context?.RequestServices ?? _serviceProvider).GetService<IOptionsMonitor<T>>();
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
        #endregion

        #region xml
        public static T GetXmlConfig<T>(string configFile, Action<T> changed = null) where T : class, IXmlConfig, new()
        {
            logger.LogDebug($"Read XML config file from: {configFile}");

            if (changed != null)
            {
                _watcher.Watch(configFile, (fileInfo) =>
                {
                    changed(GetXmlConfig<T>(configFile));
                });
            }

            var config = new T();
            config.LoadConfig(configFile);
            return config;
        }

        public static T GetXmlConfigData<T>(XmlElement ele, Func<T> action = null) where T : class, IXmlConfigData, new()
        {
            var config = action?.Invoke();
            if (config == null) config = new T();
            config.Load(ele);

            logger.LogDebug($"Get config data [{ele.Name}.{ele.GetAttribute("id")}] to: {typeof(T)}");
            return config;
        }

        public static IDictionary<string, string> GetXmlConfigParams(XmlElement ele, string name)
        {
            var dict = new Dictionary<string, string>();
            foreach (XmlElement e in ele.SelectNodes($"./{name}"))
            {
                dict.Add(e.GetAttribute("name"), e.GetAttribute("value"));
            }
            return dict;
        }

        public static IList<string> GetXmlConfigList(XmlElement ele, string name, string attr)
        {
            var list = new List<string>();
            foreach (XmlElement e in ele.SelectNodes($"./{name}"))
            {
                list.Add(e.GetAttribute(attr));
            }
            return list;
        }
        #endregion

        public static IServiceProvider UseConfigContext(this IServiceProvider service)
        {
            _watcher = service.GetRequiredService<FileWatcher>();

            return _serviceProvider = service;
        }
    }
}

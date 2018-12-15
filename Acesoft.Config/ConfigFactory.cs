using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.IO;
using System.Xml;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Acesoft.NetCore.Core;
using Acesoft.NetCore.Logging;
using Acesoft.NetCore.Util;

namespace Acesoft.NetCore.Config
{
    public sealed class ConfigFactory
    {
        static ConcurrentDictionary<string, IConfigurationRoot> configs = new ConcurrentDictionary<string, IConfigurationRoot>();
        static readonly ILogger logger = LoggerContext.GetLogger<ConfigFactory>();

        private ConfigFactory()
        { }

        public static T GetConfig<T>() where T : class, IConfig, new()
        {
            var config = new T();//Dynamic.GetInstanceCreator<T>()();
            IConfigurationRoot cfg = null;

            if (!configs.TryGetValue(config.ConfigFile, out cfg))
            {
                try
                {
                    cfg = new ConfigurationBuilder()
                        .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "config"))
                        .AddJsonFile(config.ConfigFile, optional: false, reloadOnChange: true)
                        .Build();
                    configs[config.ConfigFile] = cfg;
                    logger.LogDebug($"Read JSON config file: {config.ConfigFile}");
                }
                catch (Exception ex)
                {                    
                    logger.LogError(ex, $"Read JSON config file: {config.ConfigFile}");
                    throw ex;
                }
            }

            config.Configuration = cfg;
            cfg.Bind(config);
            return config;
        }

        public static T GetXmlConfig<T>(string configFile, Action<T> action = null) where T : class, IXmlConfig, new()
        {
            logger.LogDebug($"Read XML config file from: {configFile}");

            if (action != null)
            {
                FileWatcher.Instance.Watch(configFile, (fileInfo) =>
                {
                    action(GetXmlConfig<T>(configFile));
                });
            }

            var config = new T();
            config.LoadConfig(configFile);
            return config;
        }

        public static T GetConfigData<T>(XmlElement ele, Func<T> action = null) where T : class, IXmlConfigData, new()
        {
            var config = action?.Invoke();
            if (config == null) config = new T();
            config.Load(ele);

            logger.LogDebug($"Get config data [{ele.Name}.{ele.GetAttribute("id")}] to: {typeof(T)}");
            return config;
        }

        public static IDictionary<string, string> GetConfigParams(XmlElement ele, string name)
        {
            var dict = new Dictionary<string, string>();
            foreach (XmlElement e in ele.SelectNodes($"./{name}"))
            {
                dict.Add(e.GetAttribute("name"), e.GetAttribute("value"));
            }
            return dict;
        }

        public static IList<string> GetConfigList(XmlElement ele, string name, string attr)
        {
            var list = new List<string>();
            foreach(XmlElement e in ele.SelectNodes($"./{name}"))
            {
                list.Add(e.GetAttribute(attr));
            }
            return list;
        }

        #region xml
        /*public static T GetConfigXml<T>(string path) where T : class, IConfig
        {
            var config = Dynamic.GetInstanceCreator<T>()();
            IConfigurationRoot cfg = null;

            if (!configs.TryGetValue(path, out cfg))
            {
                try
                {
                    cfg = new ConfigurationBuilder()
                        .AddXmlFile(path, optional: true, reloadOnChange: true)
                        //.Add<XmlSource>(s => 
                        //{
                        //    s.Path = configPath;
                        //    s.Optional = false;
                        //    s.ReloadOnChange = true;
                        //    s.ResolveFileProvider();
                        //})
                        .Build();
                    configs[path] = cfg;
                    logger.LogDebug($"Read XML config file from [{path}]!");
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, $"Read XML config file [{path}] with error!");
                    throw ex;
                }
            }

            config.Configuration = cfg;
            config.ConfigFile = path;
            cfg.Bind(config);
            return config;
        }*/
        #endregion
    }
}

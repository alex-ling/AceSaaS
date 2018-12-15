using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Extensions.Configuration;

namespace Acesoft.NetCore.Config
{
    public class BaseConfig : IConfig
    {
        public string ConfigFile { get; set; }
        public IConfigurationRoot Configuration { get; set; }

        public BaseConfig()
        { }

        public T GetValue<T>(string key)
        {
            return Configuration.GetValue<T>(key);
        }

        public T GetSection<T>(string key)
        {
            return Configuration.GetSection(key).Get<T>();
        }

        public List<T> GetList<T>(string key)
        {
            return Configuration.GetSection(key).Get<List<T>>();
        }

        public Dictionary<string, T> GetDict<T>(string key)
        {
            return Configuration.GetSection(key).Get<Dictionary<string, T>>();
        }
    }
}

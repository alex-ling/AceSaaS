using Acesoft.NetCore.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Acesoft.NetCore.Config
{
    public class AppConfig : BaseConfig
    {
        public IDictionary<string, string> Settings { get; set; }

        public IDictionary<string, IDictionary<string, string>> Services { get; set; }

        public AppDataConfig Applications { get; set; }

        public AppConfig()
        {
            ConfigFile = "appconfig.json";
        }
    }

    public class AppDataConfig
    {
        public string Default { get; set; }

        public IDictionary<string, IDictionary<string, string>> AppList { get; set; }
    }
}

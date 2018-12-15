using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace Acesoft.Config
{
    public class ConfigOption
    {
        public string Name { get; set; }
        public string ConfigFile { get; set; }
        public string ConfigPath { get; set; } = "config";
        public bool Optional { get; set; }
        public bool TenantConfig { get; set; }
        public Action<BinderOptions> Binder { get; set; }
    }
}

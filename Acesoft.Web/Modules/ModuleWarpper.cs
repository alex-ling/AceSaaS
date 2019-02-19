using System;
using System.Collections.Generic;
using System.Text;

using Acesoft.Core;

namespace Acesoft.Web.Modules
{
    public class ModuleWarpper
    {
        public string Name { get; }
        public ModuleConfig ModuleConfig { get; }
        public IStartup Startup { get; }

        public ModuleWarpper(ModuleConfig module, IStartup startup)
        {
            Name = module.Name;
            ModuleConfig = module;
            Startup = startup;
        }
    }
}

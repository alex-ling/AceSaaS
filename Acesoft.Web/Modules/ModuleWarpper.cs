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
        public Type StartupType { get; }

        public ModuleWarpper(ModuleConfig module, Type startupType)
        {
            Name = module.Name;
            ModuleConfig = module;
            StartupType = startupType;
        }
    }
}

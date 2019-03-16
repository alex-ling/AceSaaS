using System;
using System.Collections.Generic;
using System.Text;

namespace Acesoft.Web.Modules
{
    public class ModuleConfig
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string MainAssembly { get; set; }
        public string Category { get; set; }
        public string Version { get; set; }
        public string[] Dependencies { get; set; }
    }
}

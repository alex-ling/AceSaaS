using System;
using System.Collections.Generic;
using System.Text;

namespace Acesoft.Web.Portal.Config
{
    public class WidgetConfig
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public string Version { get; set; }
        public string Remark { get; set; }

        public IList<TemplateConfig> Templates { get; set; }
    }

    public class TemplateConfig
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public string Remark { get; set; }
    }
}

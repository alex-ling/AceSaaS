using System;
using System.Collections.Generic;
using System.Text;

namespace Acesoft.Web.IoT.Config
{
    public class IotConfig
    {
        public IDictionary<string, IDictionary<string, string>> Settings { get; set; }
        public IDictionary<string, IotAccess> Servers { get; set; }
    }
}

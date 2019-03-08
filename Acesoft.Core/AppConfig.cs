using System;
using System.Collections.Generic;
using System.Text;

namespace Acesoft
{
    public class AppConfig
    {
        public IDictionary<string, string> Settings { get; set; }
        public IDictionary<string, IDictionary<string, string>> Services { get; set; }
    }
}

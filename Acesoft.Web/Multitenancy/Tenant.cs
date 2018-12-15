using System;
using System.Collections.Generic;
using System.Text;

namespace Acesoft.Web.Multitenancy
{
    public class Tenant
    {
        public string Name { get; set; }
        public string[] Hostnames { get; set; }
        public string[] Modules { get; set; }
        public string Theme { get; set; }
    }
}

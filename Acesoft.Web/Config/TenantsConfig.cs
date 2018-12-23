using System;
using System.Collections.Generic;
using System.Text;

namespace Acesoft.Web.Multitenancy
{
    public class TenantsConfig
    {
        public string DefaultTenant { get; set; }
        public string UnresolvedRedirect { get; set; }
        public bool RedirectPermanent { get; set; }
        public Tenant[] Tenants { get; set; }
    }
}

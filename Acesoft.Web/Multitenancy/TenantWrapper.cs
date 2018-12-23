using System;
using System.Collections.Generic;
using System.Text;

namespace Acesoft.Web.Multitenancy
{
    public class TenantWrapper : ITenant
    {
        public TenantWrapper(Tenant tenant)
        {
            Value = tenant;
        }

		public Tenant Value { get; }
    }
}

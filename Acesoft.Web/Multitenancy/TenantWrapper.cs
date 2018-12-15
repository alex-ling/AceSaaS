using System;
using System.Collections.Generic;
using System.Text;

namespace Acesoft.Web.Multitenancy
{
    public class TenantWrapper<T> : ITenant<T>
    {
        public TenantWrapper(T tenant)
        {
            Value = tenant;
        }

		public T Value { get; }
    }
}

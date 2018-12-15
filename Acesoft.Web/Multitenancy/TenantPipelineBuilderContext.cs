using System;
using System.Collections.Generic;
using System.Text;

namespace Acesoft.Web.Multitenancy
{
    public class TenantPipelineBuilderContext<T>
    {
        public TenantContext<T> TenantContext { get; set; }
        public T Tenant { get; set; }
    }
}

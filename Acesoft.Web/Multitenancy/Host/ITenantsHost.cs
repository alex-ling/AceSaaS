using System;
using System.Collections.Generic;
using System.Text;

namespace Acesoft.Web.Multitenancy
{
    public interface ITenantsHost
    {
        void Initialize();

        TenantContext GetOrCreateContext(Tenant tenant);

        void ReloadContext(Tenant tenant);
    }
}

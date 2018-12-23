using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Acesoft.Web.Multitenancy
{
    public interface ITenantContainerFactory
    {
        IServiceProvider CreateContainer(Tenant tenant);
    }
}

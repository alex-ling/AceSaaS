using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;

namespace Acesoft.Web.Multitenancy
{
    public interface ITenantResolver
    {
        Task<Tenant> ResolveAsync(HttpContext context);
    }
}

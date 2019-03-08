using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using Acesoft.Rbac;
using Acesoft.Web.Multitenancy;

namespace Acesoft.Web
{
    public interface IApplicationContext
    {
        IHostingEnvironment HostingEnvironment { get; }

        HttpContext HttpContext { get; }
        TenantContext TenantContext { get; }
        IAccessControl AC { get; }
        Acesoft.Data.ISession Session { get; }

        T As<T>() where T : class, IApplicationContext;
    }
}

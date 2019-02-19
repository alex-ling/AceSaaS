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
        HttpContext HttpContext { get; }
        TenantContext TenantContext { get; }
        Tenant Tenant { get; }

        Acesoft.Data.ISession DbSession { get; }

        IUser CurrentUser { get; }
        IUser CurrentCustomer { get; }
        IHostingEnvironment HostingEnvironment { get; }
        bool IsAuthenticated { get; }

        T As<T>() where T : class, IApplicationContext;
        T Get<T>(string name);
        void Set(string name, object value);
    }
}

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using AD = Acesoft.Data;
using Acesoft.Rbac;
using Acesoft.Web.Multitenancy;

namespace Acesoft.Web
{
    public class ApplicationContext : IApplicationContext
    {
        private IHttpContextAccessor httpContextAccessor;

        public IHostingEnvironment HostingEnvironment { get; }
        public HttpContext HttpContext => httpContextAccessor.HttpContext;
        public TenantContext TenantContext => HttpContext.GetTenantContext();
        public IAccessControl AccessControl => HttpContext.RequestServices.GetService<IAccessControl>();
        public AD.ISession Session => HttpContext.RequestServices.GetService<AD.ISession>();

        public ApplicationContext(IHostingEnvironment hostingEnvironment, IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.HostingEnvironment = hostingEnvironment;
        }
        
        public T As<T>() where T : class, IApplicationContext
        {
            return this as T;
        }
    }
}

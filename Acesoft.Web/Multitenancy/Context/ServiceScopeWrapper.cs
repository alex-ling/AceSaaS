using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Acesoft.Web.Multitenancy
{
    internal class ServiceScopeWrapper : IServiceScope
    {
        private readonly IServiceScope serviceScope;
        private readonly IServiceProvider existingServices;
        private readonly HttpContext httpContext;

        public ServiceScopeWrapper(IServiceScope serviceScope)
        {
            ServiceProvider = serviceScope.ServiceProvider;

            this.serviceScope = serviceScope;
            this.httpContext = ServiceProvider.GetRequiredService<IHttpContextAccessor>().HttpContext;
            this.existingServices = httpContext.RequestServices;
            this.httpContext.RequestServices = ServiceProvider;
        }

        public IServiceProvider ServiceProvider { get; }

        public void Dispose()
        {
            httpContext.RequestServices = existingServices;
            serviceScope.Dispose();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Builder.Internal;
using Microsoft.AspNetCore.Mvc.Internal;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace Acesoft.Web.Multitenancy
{
    public class TenantRouteBuilder : ITenantRouteBuilder
    {
        private readonly IServiceProvider services;

        // Register one top level TenantRoute per tenant. 
        // Each instance contains all the routes for this tenant.
        public TenantRouteBuilder(IServiceProvider serviceProvider)
        {
            services = serviceProvider;
        }

        public IRouteBuilder Build()
        {
            IApplicationBuilder appBuilder = new ApplicationBuilder(services);

            var routeBuilder = new RouteBuilder(appBuilder)
            {
                DefaultHandler = services.GetRequiredService<MvcRouteHandler>()
            };

            return routeBuilder;
        }

        public void Configure(IRouteBuilder builder)
        {

        }
    }
}

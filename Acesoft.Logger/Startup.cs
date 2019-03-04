using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace Acesoft.Logger
{
    public class Startup : StartupBase
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddLogging();
        }

        public override void Configure(IApplicationBuilder app, IRouteBuilder routes, IServiceProvider services)
        {
            services.UseLoggerContext();
        }
    }
}

using System;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace Acesoft.Web.HR
{
    public class Startup : StartupBase
    {
        public override void ConfigureServices(IServiceCollection services)
        {
        }

        public override void Configure(IApplicationBuilder app, IRouteBuilder routes, IServiceProvider services)
        {
            //routes.MapRoute("", async (context) =>
            //{
            //    await context.Response.WriteAsync("Hello HR!");
            //});
        }
    }
}

using System;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Http;

using Acesoft.Core;
using Acesoft.Data;

namespace Acesoft.Web.HR
{
    public class Startup : StartupBase
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IEntity, User>();
        }

        public override void Configure(IApplicationBuilder app, IRouteBuilder routes, IServiceProvider services)
        {
            //routes.MapRoute("", async (context) =>
            //{
            //    await context.Response.WriteAsync("Hello HR!");
            //});
        }
    }

    public class User : EntityBase
    {
    }
}

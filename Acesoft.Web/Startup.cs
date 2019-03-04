using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Acesoft.Data;
using Acesoft.Web.DataAccess;
using Acesoft.Web.Database;

namespace Acesoft.Web
{
    public class Startup : StartupBase
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddDataAccess();

            services.AddSingleton<IApplicationContext, ApplicationContext>();
            services.AddSingleton<ISchemaStore, SchemaStore>();
        }

        public override void Configure(IApplicationBuilder app, IRouteBuilder routes, IServiceProvider services)
        {
        }
    }
}

using System;
using System.Security.Cryptography.X509Certificates;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Http;
using IdentityServer4.Configuration;
using IdentityServer4.Validation;
using Acesoft.Config;
using Acesoft.Web.App.Config;
using Acesoft.Web.App.Services;
using Microsoft.AspNetCore.Authentication;

namespace Acesoft.Web.App
{
    public class Startup : StartupBase
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            // ∂¡»°≈‰÷√Œƒº˛
            var authConfig = ConfigContext.GetJsonConfig<AuthConfig>(opts =>
            {
                opts.Optional = true;
                opts.ConfigFile = "auth.config.json";
            });

            
        }

        public override void Configure(IApplicationBuilder app, IRouteBuilder routes, IServiceProvider services)
        {
            app.UseIdentityServer();
        }
    }
}

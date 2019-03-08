using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Acesoft.Web.Multitenancy;
using Acesoft.Config;
using Acesoft.Rbac;

namespace Acesoft.Web.Mvc
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            // app global config.
            services.Configure<AppConfig>(Configuration);
            var appConfig = Configuration.Get<AppConfig>();
            var settings = appConfig.Settings;

            // add cookie authentication.
            services.AddAuthentication(Membership.Auth_Cookie)
                .AddCookie(Membership.Auth_Cookie, opts =>
                {
                    opts.LoginPath = settings.GetValue("auth.loginurl", "/plat/account/login");
                    opts.LogoutPath = settings.GetValue("auth.logouturl", "/plat/account/logout");
                    opts.AccessDeniedPath = settings.GetValue("auth.denyurl", "/plat/account/deny");
                    opts.ExpireTimeSpan = TimeSpan.FromDays(settings.GetValue("auth.expiredays", 15));
                    opts.SlidingExpiration = true;
                });

            services.AddMultitenancy();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            // use status code to error page.
            app.UseStatusCodePagesWithReExecute("/error/{0}");

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/error/500");
                app.UseHsts();
            }

            // Use SaaS middleware.
            app.UseMultitenancy();

            /*app.UsePerTenant((ctx, builder) =>
            {
                // for per tenant config                
            });*/
        }
    }
}

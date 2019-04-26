using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication;
using Acesoft.Web.Multitenancy;
using Acesoft.Config;
using Acesoft.Rbac;
using Acesoft.Web.Middleware;
using Acesoft.Web.WeChat.Authenticatoon;

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
            //services.AddSingleton(Configuration);

            // global config.
            App.SetAppConfig(Configuration.Get<AppConfig>());

            // add SaaS
            services.AddMultitenancy();

            // add authentication
            var settings = App.AppConfig.Settings;
            services.AddAuthentication(Membership.Auth_Cookie)
            .AddCookie(Membership.Auth_Cookie, opts =>
            {
                // Web local auth
                opts.ReturnUrlParameter = "returnurl";
                opts.LoginPath = settings.GetValue("auth.loginurl", "/plat/account/login");
                opts.LogoutPath = settings.GetValue("auth.logouturl", "/plat/account/logout");
                opts.AccessDeniedPath = settings.GetValue("auth.denyurl", "/plat/account/deny");
                opts.ExpireTimeSpan = TimeSpan.FromDays(settings.GetValue("auth.expiredays", 15));
                opts.SlidingExpiration = true;
            })
            .AddIdentityServerAuthentication(opts =>
            {
                // Bearer auth for client api
                opts.Authority = settings.GetValue("oauth.server", App.GetWebRoot(true));
                opts.RequireHttpsMetadata = settings.GetValue("oauth.https", false);
                opts.ApiName = settings.GetValue("oauth.apiscope", "api");
            })
            .AddWechat();

            // set AppConfig for null to auto refresh
            App.SetAppConfig();
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
                //app.UseHsts();
            }

            // use anthentication
            app.UseAuthentication();

            // Use SaaS middleware.
            app.UseMultitenancy();

            // for per tenant config                
            /*app.UsePerTenant((ctx, builder) =>
            {                
            });*/
        }
    }
}

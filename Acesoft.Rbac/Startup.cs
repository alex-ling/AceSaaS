using System;
using System.Security.Cryptography.X509Certificates;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Http;
using IdentityServer4.Validation;
using IdentityServer4.Configuration;
using Acesoft.Data;
using Acesoft.Rbac.Services;
using Acesoft.Rbac.StateProviders;
using Acesoft.Rbac.Schema;
using Acesoft.Rbac.Config;

namespace Acesoft.Rbac
{
    public class Startup : StartupBase
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            // regist states
            services.AddSingleton<IStateProvider, UserStateProvider>();

            // regist schema
            services.AddSingleton<IStoreSchema, RbacSchema>();

            // regist services
            services.AddSingleton<IObjectService, ObjectService>();
            services.AddSingleton<IRoleService, RoleService>();
            services.AddSingleton<IPAService, PAService>();
            services.AddSingleton<IUserService, UserService>();
            services.AddSingleton<IUAService, UAService>();
            services.AddSingleton<IUOService, UOService>();
            services.AddSingleton<IScaleService, ScaleService>();

            // regist AccessControl to DI for requtest scope
            services.AddScoped<IAccessControl, AccessControl>();

            // add oauth client. https://identitymodel.readthedocs.io/en/latest/client/discovery.html
            //services.AddSingleton<IDiscoveryCache>(r =>
            //{
            //    var factory = r.GetRequiredService<IHttpClientFactory>();
            //    return new DiscoveryCache(Constants.Authority, () => factory.CreateClient());
            //});

            // add identityserver
            var settings = App.AppConfig.Settings;
            var certPath = settings.GetValue<string>("oauth.certpath");
            var certPwd = settings.GetValue<string>("oauth.certpwd");
            services.AddTransient<IResourceOwnerPasswordValidator, UserValidator>()
            .AddIdentityServer(opts =>
            {                
                opts.Authentication = new AuthenticationOptions
                {
                    //CheckSessionCookieName = settings.GetValue("oauth.authcookie", "AceAuth"),
                    CookieLifetime = TimeSpan.FromDays(settings.GetValue("oauth.expiredays", 15)),
                    CookieSlidingExpiration = true,
                    RequireAuthenticatedUserForSignOutMessage = true
                };
                opts.Caching = new CachingOptions
                {
                    ClientStoreExpiration = TimeSpan.FromHours(24.0),
                    ResourceStoreExpiration = TimeSpan.FromHours(24.0),
                    CorsExpiration = TimeSpan.FromHours(24.0 * 15)
                };
                #region option
                /*opts.Events = new EventsOptions
                {
                    RaiseErrorEvents = true,
                    RaiseFailureEvents = true,
                    RaiseSuccessEvents = true,
                    RaiseInformationEvents = true
                };
                opts.InputLengthRestrictions = new InputLengthRestrictions
                {
                    AcrValues = 100,
                    AuthorizationCode = 100,
                    ClientId = 100,
                    ClientSecret = 1000
                };
                opts.UserInteraction = new UserInteractionOptions
                {
                    LoginReturnUrlParameter = "returnurl",
                    LoginUrl = settings.GetValue("auth.loginurl", "/plat/account/login"),
                    LogoutUrl = settings.GetValue("auth.logouturl", "/plat/account/logout")
                };                
                opts.Cors = new CorsOptions
                {
                    CorsPaths = new PathString[] { "/" },
                    CorsPolicyName = "all",
                    PreflightCacheDuration = new TimeSpan(1, 0, 0)
                };*/
                #endregion
            })
            .AddSigningCredential(new X509Certificate2(certPath, certPwd))
            .AddClientStore<ClientStore>()
            .AddInMemoryApiResources(IS4Config.GetApiResources());
        }

        public override void Configure(IApplicationBuilder app, IRouteBuilder routes, IServiceProvider services)
        {
            // add id4
            app.UseIdentityServer();
        }
    }
}
   
using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

using Acesoft.Data;
using Acesoft.Rbac.Services;
using Acesoft.Rbac.StateProviders;
using Acesoft.Rbac.Schema;
using IdentityModel.Client;
using System.Net.Http;

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
            services.AddSingleton<IScaleService, ScaleService>();

            // regist AccessControl to DI for requtest scope
            services.AddScoped<IAccessControl, AccessControl>();

            // add oauth client. https://identitymodel.readthedocs.io/en/latest/client/discovery.html
            //services.AddSingleton<IDiscoveryCache>(r =>
            //{
            //    var factory = r.GetRequiredService<IHttpClientFactory>();
            //    return new DiscoveryCache(Constants.Authority, () => factory.CreateClient());
            //});
        }

        public override void Configure(IApplicationBuilder app, IRouteBuilder routes, IServiceProvider services)
        {

        }
    }
}
   
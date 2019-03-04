using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

using Acesoft.Rbac.Services;
using Acesoft.Rbac.StateProviders;

namespace Acesoft.Rbac
{
    public class Startup : StartupBase
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            // regist states
            services.AddSingleton<IStateProvider, UserStateProvider>();

            // regist services
            services.AddSingleton<IObjectService, ObjectService>();
            services.AddSingleton<IRoleService, RoleService>();
            services.AddSingleton<IPAService, PAService>();
            services.AddSingleton<IUserService, UserService>();
            services.AddSingleton<IUAService, UAService>();
            services.AddSingleton<IScaleService, ScaleService>();

            // regist AccessControl to DI for requtest scope
            services.AddScoped<IAccessControl, AccessControl>();
        }

        public override void Configure(IApplicationBuilder app, IRouteBuilder routes, IServiceProvider services)
        {

        }
    }
}
   
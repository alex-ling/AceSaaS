using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Routing;
using Acesoft.Web.Multitenancy;
using Acesoft.Web.Modules;

namespace Acesoft.Web
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseMultitenancy(this IApplicationBuilder app)
        {
            var services = app.ApplicationServices;
            var env = services.GetRequiredService<IHostingEnvironment>();

            // set global HttpContext
            services.UseAppContext();

            // use common
            //app.UseHttpsRedirection();
            app.UseStaticFiles();

            // use multitenant
            app.UseMiddleware<TenantContainerMiddleware>();

            // use multitenant route
            app.UseMiddleware<TenantRouterMiddleware>();

            // use mvc
            app.UseMvc(routes =>
            {
                // use internal/core modules
                ModuleLoader.Configure(app, routes, services);

                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}"
                );

                routes.MapRoute(
                    name: "areas",
                    template: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
                );
            });

            return app;
        }

        public static IApplicationBuilder UsePerTenant(this IApplicationBuilder app,       
            Action<TenantContext, IApplicationBuilder> configure)
        {
            // 按每个tenant进行注册、配置
            return app.Use(next => new TenantPipelineMiddleware(next, app, configure).Invoke);
        }
    }
}

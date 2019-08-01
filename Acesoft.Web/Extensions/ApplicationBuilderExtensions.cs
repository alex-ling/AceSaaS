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
using Microsoft.Extensions.FileProviders;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.StaticFiles;
using Acesoft.Web.Middleware;

namespace Acesoft.Web
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseMultitenancy(this IApplicationBuilder app)
        {
            var services = app.ApplicationServices;
            var env = services.GetRequiredService<IHostingEnvironment>();

            // cookie
            app.UseCookiePolicy();

            // use webapi result to request.
            app.UseMiddleware<ExceptionMiddleware>();

            //app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseStaticFiles(new StaticFileOptions
            {
                ContentTypeProvider = new FileExtensionContentTypeProvider(new Dictionary<string, string>
                {
                    {
                        ".json",
                        "application/json"
                    },
                    {
                        ".apk",
                        "application/vnd.android.package-archive"
                    },
                    {
                        ".nupkg",
                        "application/zip"
                    },
                    {
                        ".exe",
                        "application/octet-stream"
                    }
                })
            });
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(App.GetLocalPath("logs", true)),
                RequestPath = new PathString("/logs")
            });
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(App.GetLocalPath("uploads", true)),
                RequestPath = new PathString("/uploads")
            });

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

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using Microsoft.AspNetCore.Hosting;
using NLog;
using NLog.LayoutRenderers;
using NLog.Web;

namespace Acesoft.Logger
{
    public static class WebHostBuilderExtensions
    {
        public static IWebHostBuilder UseNLogWeb(this IWebHostBuilder builder)
        {
            LayoutRenderer.Register<TenantLayoutRenderer>(TenantLayoutRenderer.LayoutRendererName);
            builder.UseNLog();
            builder.ConfigureAppConfiguration((context, configuration) =>
            {
                var environment = context.HostingEnvironment;
                environment.ConfigureNLog($"{environment.ContentRootPath}{Path.DirectorySeparatorChar}nlog.config");
                LogManager.Configuration.Variables["configDir"] = environment.ContentRootPath;
            });

            return builder;
        }
    }
}

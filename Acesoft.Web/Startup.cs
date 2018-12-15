using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc;

using Acesoft.Core;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace Acesoft.Web
{
    public class Startup : StartupBase
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            #region AddMvc
            services.AddMvc(opts =>
            {
                // mvc设置
            })
            .AddRazorOptions(opts =>
            {
                // 此处可添加外部views目录
                //opts.FileProviders.Add(new PhysicalFileProvider(""));
            })
            .AddRazorPagesOptions(opts =>
            {
                // 此处设置RazorPages
            })
            .SetCompatibilityVersion(CompatibilityVersion.Latest);
            #endregion
        }

        public override void Configure(IApplicationBuilder app, IRouteBuilder routes, IServiceProvider services)
        {
            app.UseHttpsRedirection();
            app.UseStaticFiles();
        }
    }
}

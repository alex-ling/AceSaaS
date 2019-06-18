using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using Acesoft.Logger;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Serilog;

namespace Acesoft.Web.Mvc
{
    public class Program
    {
        public static void Main(string[] args)
        {
            LoggerContext.RunMainWithSerilog(() =>
            {
                CreateWebHostBuilder(args).Build().Run();
            });
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                //.UseAppConfig()
                .UseStartup<Startup>()
                .UseSerilog();
    }
}

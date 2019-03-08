using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace Acesoft.Web
{
    public static class IWebHostBuilderExtensions
    {
        public static IWebHostBuilder UseAppConfig(this IWebHostBuilder builder)
        {
            return builder.ConfigureAppConfiguration((hostContext, config) =>
            {
                config.AddJsonFile("config/app.config.json", false, true);
            });
        }
    }
}

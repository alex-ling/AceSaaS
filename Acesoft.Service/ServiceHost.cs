using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Acesoft.Core;
//using Acesoft.Service.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using Acesoft.Logger;
using Acesoft.Config;
using Acesoft.Service.Config;

namespace Acesoft.Service
{
    public static class ServiceHost
    {
        public static IServiceProvider BuildService()
        {
            var services = new ServiceCollection()
                .AddOptions();

            //services.AddTransient<ILoggerFactory, LoggerFactory>();
            //services.AddTransient<IServiceBase, TestService>();

            return services.BuildServiceProvider();
        }
    }
}

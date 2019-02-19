using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Acesoft.Logger
{
    public static class LoggerContext
    {
        static ILoggerFactory factory;

        public static ILogger GetLogger(string name)
        {
            return factory.CreateLogger(name);
        }

        public static ILogger<T> GetLogger<T>()
        {
            return factory.CreateLogger<T>();
        }

        public static IServiceProvider UseLoggerContext(this IServiceProvider service)
        {
            factory = service.GetService<ILoggerFactory>();
            return service;
        }
    }
}

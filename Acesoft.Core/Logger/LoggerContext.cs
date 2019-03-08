using System;
using System.Collections.Generic;
using System.IO;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MS = Microsoft.Extensions.Logging;
using Serilog;

namespace Acesoft.Logger
{
    public static class LoggerContext
    {
        static ILoggerFactory factory;

        public static MS.ILogger GetLogger(string name)
        {
            return factory.CreateLogger(name);
        }

        public static MS.ILogger<T> GetLogger<T>()
        {
            return factory.CreateLogger<T>();
        }

        public static Serilog.ILogger GetSerilogLogger => Log.Logger;

        public static IServiceProvider UseLoggerContext(this IServiceProvider service)
        {
            factory = service.GetService<ILoggerFactory>();
            return service;
        }

        public static void RunMainWithSerilog(Action main)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "config"))
                .AddJsonFile("serilog.config.json", optional: false, reloadOnChange: true)
                .Build();

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .CreateLogger();

            try
            {
                Log.Information("Starting web host");

                main();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.ServiceProcess;

using PeterKottas.DotNetCore.WindowsService;
using Acesoft.Service.Services;

namespace Acesoft.Service
{
    public class Program
    {
        static void Main(string[] args)
        {
            ServiceRunner<TestService>.Run(config =>
            {
                var name = config.GetDefaultName();

                config.Service(sc =>
                {
                    sc.ServiceFactory((extraArguments, controller) =>
                    {
                        return new TestService();
                    });

                    sc.OnStart((service, extraArguments) =>
                    {
                        Console.WriteLine("Service {0} started", name);
                        service.Start();
                    });

                    sc.OnStop(service =>
                    {
                        Console.WriteLine("Service {0} stopped", name);
                        service.Stop();
                    });

                    sc.OnError(e =>
                    {
                        Console.WriteLine("Service {0} errored with exception : {1}", name, e.Message);
                    });
                });

                config.SetName("AcesoftIotService");
                config.SetDisplayName("Acesoft Iot Service");
                config.SetDescription("Acesoft Iot Service is a cloud TCP/IP access gateway for responding to smart devices");
            });
        }
    }
}

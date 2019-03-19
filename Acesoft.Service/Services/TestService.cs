using System;
using System.Collections.Generic;
using System.Text;
using System.Timers;

using PeterKottas.DotNetCore.WindowsService.Interfaces;
using PeterKottas.DotNetCore.WindowsService.Base;
using Microsoft.Extensions.Logging;
using Acesoft.Core;
using Acesoft.Logger;
using System.Threading;

namespace Acesoft.Service.Services
{
    public class TestService : MicroService, IServiceBase
    {
        //private readonly ILogger logger = LoggerContext.GetLogger<TestService>();

        public void Start()
        {
            StartBase();

            Timers.Start("Poller", 1000, () =>
            {
                Console.WriteLine("Polling at {0}\n", DateTime.Now.ToString("o"));
            },
            (e) =>
            {
                Console.WriteLine("Exception while polling: {0}\n", e.ToString());
            });

            Console.WriteLine("I started");
        }

        public void Stop()
        {
            StopBase();

            Console.WriteLine("I stopped");
        }
    }
}

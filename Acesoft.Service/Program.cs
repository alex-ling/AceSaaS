using System;

using Microsoft.Extensions.DependencyInjection;

namespace Acesoft.App.Service
{
    public class Program
    {
        static void Main(string[] args)
        {
            BuildApp();

            Console.ReadLine();
        }

        static ServiceProvider BuildApp()
        {
            return new ServiceCollection()
                .BuildServiceProvider();
        }
    }
}

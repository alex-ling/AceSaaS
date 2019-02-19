using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Extensions.DependencyInjection;
using Acesoft.Config;
using Acesoft.Util;

namespace Acesoft.Console.Test
{
    public class ConfigMonitorTest
    {
        public static void MonitorTest()
        {
            var services = new ServiceCollection()
                .AddJsonConfig<TestConfig>(opts =>
                {
                    opts.IsTenantConfig = true;
                    opts.ConfigFile = "tenant.json";
                })
                .BuildServiceProvider()
                .UseConfigContext();

            var testConfig = ConfigContext.GetConfig<TestConfig>("test", (cfg, name) =>
            {
                System.Console.WriteLine($"{name} config has changed");
                System.Console.WriteLine($"Now the config file's list: {cfg.SubConfig.List.Join()}");
            });

            System.Console.WriteLine($"Now the config file's list: {testConfig.SubConfig.List.Join()}");
        }
    }

    public class TestConfig
    {
        public string Name { get; set; }
        public int Number { get; set; }
        public string[] Array { get; set; }
        public Dictionary<string, object> Dict { get; set; }
        public SubConfig SubConfig { get; set; }
    }

    public class SubConfig
    {
        public string SubName { get; set; }
        public List<int> List { get; set; }
    }
}

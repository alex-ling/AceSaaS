using System;
using System.Collections.Generic;

using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using Acesoft.Config;

namespace Acesoft.Core.Tests
{
    public class ConfigTests
    {
        IServiceCollection _services;

        [SetUp]
        public void Setup()
        {
            _services = new ServiceCollection();
        }

        [Test]
        public void ConfigTest()
        {
            _services.AddJsonConfig<TestConfig>(opts =>
            {
                opts.ConfigFile = "test.json";
            })
            .BuildServiceProvider()
            .UseConfigContext();

            var testConfig = ConfigContext.GetConfig<TestConfig>();

            Assert.AreNotEqual(testConfig, null);
            Assert.AreEqual(testConfig.Name, "hello");
        }

        [Test]
        public void ConfigWithNameTest()
        {
            _services.AddJsonConfig<TestConfig>(opts =>
            {
                opts.Name = "test";
                opts.ConfigFile = "test.json";
            })
            .BuildServiceProvider()
            .UseConfigContext();

            var testConfig = ConfigContext.GetConfig<TestConfig>("test");

            Assert.AreNotEqual(testConfig, null);
            Assert.AreEqual(testConfig.SubConfig.SubName, "world");
            Assert.AreEqual(testConfig.SubConfig.List[4], 4);
        }

        [Test]
        public void ConfigWithTenantTest()
        {
            _services.AddJsonConfig<TestConfig>(opts =>
            {
                opts.TenantConfig = true;
                opts.ConfigFile = "tenant.json";
            })
            .BuildServiceProvider()
            .UseConfigContext();

            var testConfig = ConfigContext.GetConfig<TestConfig>("test");
            Assert.AreNotEqual(testConfig, null);
            Assert.AreEqual(testConfig.SubConfig.SubName, "world");
            Assert.AreEqual(testConfig.SubConfig.List.Count, 5);

            var webConfig = ConfigContext.GetConfig<TestConfig>("web");
            Assert.AreNotEqual(webConfig, null);
            Assert.AreEqual(webConfig.SubConfig.SubName, "web");
            Assert.AreEqual(webConfig.SubConfig.List.Count, 4);
        }

        [Test]
        public void ConfigWithNotBuild()
        {
            var testConfig = _services.GetJsonConfig<TestConfig>(opts =>
                {
                    opts.Name = "test";
                    opts.ConfigFile = "test.json";
                });

            Assert.AreNotEqual(testConfig, null);
            Assert.AreEqual(testConfig.SubConfig.SubName, "world");
            Assert.AreEqual(testConfig.SubConfig.List[4], 4);
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
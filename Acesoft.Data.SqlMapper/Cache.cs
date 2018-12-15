using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml;

using Acesoft.NetCore.Core;
using Acesoft.NetCore.Config;
using Acesoft.Data.SqlMapper.Caching;

namespace Acesoft.Data.SqlMapper
{
    public class Cache : XmlConfigData
    {
        public SqlScope Scope { get; set; }
        public string Id { get; private set; }
        public string Type { get; private set; }
        public IDictionary<string, string> Params { get; private set; }
        public IList<string> FlushOnExecutes { get; private set; }
        public ICacheProvider Provider { get; private set; }
        public TimeSpan FlushInterval { get; private set; }

        private ICacheProvider CreateCacheProvider()
        {
            ICacheProvider provider = null;
            if (Type.HasValue())
            {
                switch (Type)
                {
                    case "lru":
                        provider = new LruCacheProvider();
                        break;
                    case "fifo":
                        provider = new FifoCacheProvider();
                        break;
                    default:
                        provider = Dynamic.GetInstanceCreator(System.Type.GetType(Type))() as ICacheProvider;
                        break;
                }
            }
            else
            {
                provider = new NoneCacheProvider();
            }
            provider.Initialize(Params);
            return provider;
        }

        public override void Load(XmlElement config)
        {
            base.Load(config);

            this.Id = config.GetAttribute("id");
            this.Type = config.GetAttribute("type");
            this.Params = ConfigFactory.GetConfigParams(config, "param");
            this.FlushOnExecutes = ConfigFactory.GetConfigList(config, "flushonexecute", "sqlmap");
            this.Provider = CreateCacheProvider();

            var flushinterval = Params.GetValue("flushinterval", 0);
            if (flushinterval > 0)
            {
                this.FlushInterval = TimeSpan.FromMinutes(flushinterval);
            }
        }
    }
}

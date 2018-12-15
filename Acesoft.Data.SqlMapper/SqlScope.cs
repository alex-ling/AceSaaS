using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using Acesoft.NetCore.Config;

namespace Acesoft.Data.SqlMapper
{
    public class SqlScope : BaseXmlConfig
    {
        public string Id { get; private set; }
        public IDictionary<string, SqlMap> SqlMaps { get; private set; }
        public IDictionary<string, Cache> Caches { get; private set; }

        public SqlScope()
        {
            SqlMaps = new Dictionary<string, SqlMap>();
            Caches = new Dictionary<string, Cache>();
        }

        public override void Load(XmlElement config)
        {
            base.Load(config);

            this.Id = config.GetAttribute("id");
            foreach (XmlElement cfg in config.SelectNodes("//cache"))
            {
                var cache = ConfigFactory.GetConfigData(cfg, () =>
                {
                    return new Cache { Scope = this };
                });
                Caches.Add(cache.Id, cache);
            }
            foreach (XmlElement cfg in config.SelectNodes("//sqlmap"))
            {
                var sqlMap = ConfigFactory.GetConfigData(cfg, () =>
                {
                    return new SqlMap { Scope = this };
                });
                SqlMaps.Add(sqlMap.Id, sqlMap);
            }
        }
    }
}

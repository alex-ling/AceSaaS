using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

using Acesoft.Config;
using Acesoft.Config.Xml;

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
                var cache = ConfigContext.GetXmlConfigData(cfg, () =>
                {
                    return new Cache { Scope = this };
                });
                Caches.Add(cache.Id, cache);
            }
            foreach (XmlElement cfg in config.SelectNodes("//sqlmap"))
            {
                var sqlMap = ConfigContext.GetXmlConfigData(cfg, () =>
                {
                    return new SqlMap { Scope = this };
                });
                SqlMaps.Add(sqlMap.Id, sqlMap);
            }
        }
    }
}

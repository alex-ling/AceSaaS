using System;
using System.Collections.Generic;
using System.Text;

using Acesoft.NetCore.Config;
using Acesoft.NetCore.Web;
using Microsoft.Extensions.Logging;
using Acesoft.NetCore.Logging;

namespace Acesoft.Data.Config
{
    public class DataConfig : BaseConfig
    {
        private static ILogger logger = LoggerContext.GetLogger<DataConfig>();

        public const string ConnectionString = "connectionstring";
        public const string DbProvider = "dbprovider";

        public string Default { get; set; }

        public IDictionary<string, IDictionary<string, string>> Databases { get; set; }
        public IDictionary<string, IList<string>> SqlMaps { get; set; }

        public DataConfig()
        {
            ConfigFile = "dataconfig.json";
        }

        public static string GetDatabase()
        {
            var db = App.GetQuery<string>("app", "");
            if (!db.HasValue())
            {
                var cfg = ConfigFactory.GetConfig<DataConfig>();
                logger.LogDebug($"Get database's name from default with: {cfg.Default}");
                return cfg.Default;
            }
            logger.LogDebug($"Get database's name from request width: {db}");
            return db;
        }
    }
}

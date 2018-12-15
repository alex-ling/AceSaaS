using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

using Acesoft.Data.Config;
using Acesoft.NetCore.Config;
using Microsoft.Extensions.Logging;
using Acesoft.NetCore.Logging;

namespace Acesoft.Data.SqlMapper
{
    public class MapperContainer
    {
        private ILogger logger;

        static MapperContainer instance = null;
        static readonly ConcurrentDictionary<string, SqlMapper> mappers = new ConcurrentDictionary<string, SqlMapper>();

        public static MapperContainer Instance => instance;

        private MapperContainer()
        {
            logger = LoggerContext.GetLogger<MapperContainer>();
        }

        static MapperContainer()
        {
            instance = new MapperContainer();
        }

        public ISqlMapper GetSqlMapper()
        {
            var mapperName = "default";
            logger.LogDebug($"Get ISqlMapper for name: [{mapperName}]");
            return mappers.GetOrAdd(mapperName, (key) =>
            {
                var config = ConfigFactory.GetConfig<DataConfig>();
                var dirs = config.SqlMaps[mapperName];

                logger.LogDebug($"Initalize ISqlMapper for name: {mapperName}");
                return new SqlMapper(dirs);
            });
        }
    }
}

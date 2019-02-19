using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

using Microsoft.Extensions.Logging;
using Acesoft.Data.Config;
using Acesoft.Logger;
using Acesoft.Config;

namespace Acesoft.Data.SqlMapper
{
    public class MapperContainer
    {
        private ILogger logger = LoggerContext.GetLogger<MapperContainer>();

        static MapperContainer instance = null;
        static readonly ConcurrentDictionary<string, SqlMapper> mappers = new ConcurrentDictionary<string, SqlMapper>();

        public static MapperContainer Instance => instance;

        private MapperContainer()
        {
        }

        static MapperContainer()
        {
            instance = new MapperContainer();
        }

        public ISqlMapper GetSqlMapper(IStoreOption option)
        {
            logger.LogDebug($"Get ISqlMapper for name: {option.Name}");
            return mappers.GetOrAdd(option.Name, (key) =>
            {
                logger.LogDebug($"Initalize ISqlMapper for name: {option.Name}");
                return new SqlMapper(option.SqlMaps);
            });
        }
    }
}

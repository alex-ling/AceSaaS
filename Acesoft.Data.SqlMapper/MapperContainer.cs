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

        public ISqlMapper GetSqlMapper(ISession session)
        {
            var database = session.Store.Option.Name;
            logger.LogDebug($"Get ISqlMapper for name: {database}");
            return mappers.GetOrAdd(database, (key) =>
            {
                logger.LogDebug($"Initalize ISqlMapper for name: {database}");
                return new SqlMapper(session.Store.Option.SqlMaps);
            });
        }
    }
}

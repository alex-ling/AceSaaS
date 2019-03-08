using System;
using System.Collections.Generic;
using System.Data;
using System.IO;

using Acesoft.Data;
using Acesoft.Data.SqlServer;
using Acesoft.Data.MySql;
using Acesoft.Data.PostgreSql;
using Acesoft.Data.Sqlite;
using Acesoft.Data.Config;

namespace Acesoft.Web.DataAccess
{
    public static class StoreOptionExtensions
    {
        public static IStoreOption UseDatabase(this IStoreOption option, string database)
        {
            option.Name = database;
            return option;
        }

        public static IStoreOption UseConfig(this IStoreOption option, DataConfig config)
        {
            option.SqlMaps = config.SqlMaps;

            switch (config.DatabaseType.ToLower())
            {
                case "sqlserver":
                    option.UseSqlServer(config.ConnectionString, IsolationLevel.ReadUncommitted);
                    break;
                case "Sqlite":
                    var databaseFolder = App.GetLocalPath("appdata/db/", true);
                    var databaseFile = Path.Combine(databaseFolder, $"{option.Name}.db");
                    option.UseSqLite($"Data Source={databaseFile};Cache=Shared", IsolationLevel.ReadUncommitted);
                    break;
                case "MySql":
                    option.UseMySql(config.ConnectionString, IsolationLevel.ReadUncommitted);
                    break;
                case "Postgres":
                    option.UsePostgreSql(config.ConnectionString, IsolationLevel.ReadUncommitted);
                    break;
                default:
                    throw new AceException("Unknown database provider: " + config.DatabaseType);
            }

            return option;
        }

        public static IStoreOption SetIsolationLevel(this IStoreOption option, IsolationLevel isolationLevel)
        {
            option.IsolationLevel = isolationLevel;
            return option;
        }

        public static IStoreOption SetConnectionFactory(this IStoreOption option, IConnectionFactory connectionFactory)
        {
            option.ConnectionFactory = connectionFactory;
            return option;
        }

        public static IStoreOption AutoCreateSchema(this IStoreOption option, bool autoCreateSchema = true)
        {
            option.AutoCreateSchema = autoCreateSchema;
            return option;
        }

        public static IStoreOption SetSessionPoolSize(this IStoreOption option, int size)
        {
            option.SessionPoolSize = size;
            return option;
        }

        /*public static IStoreOption DisableQueryGating(this IStoreOption option)
        {
            option.QueryGatingEnabled = false;
            return option;
        }*/

        public static IStoreOption SetSqlMaps(this IStoreOption option, string[] sqlmaps)
        {
            option.SqlMaps = sqlmaps;
            return option;
        }
    }
}

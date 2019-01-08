using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

using Microsoft.Data.Sqlite;
using Acesoft.Data.Config;
using Acesoft.Data.Sql;

namespace Acesoft.Data.Sqlite
{
    public static class SqliteExtensions
    {
        public static IConfiguration RegisterSqLite(this IConfiguration configuration)
        {
            SqlDialectFactory.SqlDialects["sqliteconnection"] = new SqliteDialect();
            CommandInterpreterFactory.CommandInterpreters["sqliteconnection"] = d => new SqliteCommandInterpreter(d);

            return configuration;
        }

        public static IConfiguration UseInMemory(this IConfiguration configuration)
        {
            const string inMemoryConnectionString = "Data Source=:memory:";
            return UseSqLite(configuration, inMemoryConnectionString, IsolationLevel.Serializable, shareConnection: true);
        }

        public static IConfiguration UseSqLite(
            this IConfiguration configuration,
            string connectionString)
        {
            return UseSqLite(
                configuration,
                connectionString,
                IsolationLevel.Serializable);
        }

        public static IConfiguration UseSqLite(
            this IConfiguration configuration,
            string connectionString,
            IsolationLevel isolationLevel,
            bool shareConnection = false)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            if (String.IsNullOrWhiteSpace(connectionString))
            {
                throw new ArgumentException(nameof(connectionString));
            }

            RegisterSqLite(configuration);
            configuration.ConnectionFactory = new DbConnectionFactory<SqliteConnection>(connectionString, shareConnection);
            configuration.IsolationLevel = isolationLevel;

            return configuration;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

using Npgsql;
using Acesoft.Data.Config;
using Acesoft.Data.Sql;

namespace Acesoft.Data.PostgreSql
{
    public static class PostgreSqlExtensions
    {
        public static IConfiguration RegisterPostgreSql(this IConfiguration configuration)
        {
            SqlDialectFactory.SqlDialects["npgsqlconnection"] = new PostgreSqlDialect();
            CommandInterpreterFactory.CommandInterpreters["npgsqlconnection"] = d => new PostgreSqlCommandInterpreter(d);

            return configuration;
        }

        public static IConfiguration UsePostgreSql(
            this IConfiguration configuration,
            string connectionString)
        {
            return UsePostgreSql(configuration, connectionString, IsolationLevel.ReadUncommitted);
        }

        public static IConfiguration UsePostgreSql(
            this IConfiguration configuration,
            string connectionString,
            IsolationLevel isolationLevel)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            if (String.IsNullOrWhiteSpace(connectionString))
            {
                throw new ArgumentException(nameof(connectionString));
            }

            RegisterPostgreSql(configuration);
            configuration.ConnectionFactory = new DbConnectionFactory<NpgsqlConnection>(connectionString);
            configuration.IsolationLevel = isolationLevel;

            return configuration;
        }
    }
}

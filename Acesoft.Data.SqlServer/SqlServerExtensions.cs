using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

using Acesoft.Data.Config;
using Acesoft.Data.Sql;

namespace Acesoft.Data.SqlServer
{
    public static class SqlServerExtensions
    {
        public static IConfiguration RegisterSqlServer(this IConfiguration configuration)
        {
            SqlDialectFactory.SqlDialects["sqlconnection"] = new SqlServerDialect();
            CommandInterpreterFactory.CommandInterpreters["sqlconnection"] = d => new SqlServerCommandInterpreter(d);

            return configuration;
        }

        public static IConfiguration UseSqlServer(
            this IConfiguration configuration,
            string connectionString)
        {
            return UseSqlServer(configuration, connectionString, IsolationLevel.ReadUncommitted);
        }

        public static IConfiguration UseSqlServer(
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

            RegisterSqlServer(configuration);
            configuration.ConnectionFactory = new DbConnectionFactory<SqlConnection>(connectionString);
            configuration.IsolationLevel = isolationLevel;

            return configuration;
        }
    }
}

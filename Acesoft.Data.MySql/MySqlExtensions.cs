using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

using MySql.Data.MySqlClient;
using Acesoft.Data.Config;
using Acesoft.Data.Sql;

namespace Acesoft.Data.MySql
{
    public static class MySqlExtensions
    {
        public static IConfiguration RegisterMySql(this IConfiguration configuration)
        {
            SqlDialectFactory.SqlDialects["mysqlconnection"] = new MySqlDialect();
            CommandInterpreterFactory.CommandInterpreters["mysqlconnection"] = d => new MySqlCommandInterpreter(d);

            return configuration;
        }

        public static IConfiguration UseMySql(
            this IConfiguration configuration,
            string connectionString)
        {
            return UseMySql(configuration, connectionString, IsolationLevel.ReadUncommitted);
        }

        public static IConfiguration UseMySql(
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

            RegisterMySql(configuration);

            configuration.ConnectionFactory = new DbConnectionFactory<MySqlConnection>(connectionString);
            configuration.IsolationLevel = isolationLevel;

            return configuration;
        }
    }
}

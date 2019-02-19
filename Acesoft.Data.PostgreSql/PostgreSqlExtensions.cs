using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

using Npgsql;
using Acesoft.Data.Sql;

namespace Acesoft.Data.PostgreSql
{
    public static class PostgreSqlExtensions
    {
        public static IStoreOption RegisterPostgreSql(this IStoreOption option)
        {
            SqlDialectFactory.SqlDialects["npgsqlconnection"] = new PostgreSqlDialect();
            CommandInterpreterFactory.CommandInterpreters["npgsqlconnection"] = d => new PostgreSqlCommandInterpreter(d);

            return option;
        }

        public static IStoreOption UsePostgreSql(
            this IStoreOption option,
            string connectionString)
        {
            return UsePostgreSql(option, connectionString, IsolationLevel.ReadUncommitted);
        }

        public static IStoreOption UsePostgreSql(
            this IStoreOption option,
            string connectionString,
            IsolationLevel isolationLevel)
        {
            if (option == null)
            {
                throw new ArgumentNullException(nameof(option));
            }

            if (String.IsNullOrWhiteSpace(connectionString))
            {
                throw new ArgumentException(nameof(connectionString));
            }

            RegisterPostgreSql(option);
            option.ConnectionFactory = new DbConnectionFactory<NpgsqlConnection>(connectionString);
            option.IsolationLevel = isolationLevel;

            return option;
        }
    }
}

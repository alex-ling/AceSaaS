using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

using MySql.Data.MySqlClient;
using Acesoft.Data.Sql;

namespace Acesoft.Data.MySql
{
    public static class MySqlExtensions
    {
        public static IStoreOption RegisterMySql(this IStoreOption option)
        {
            SqlDialectFactory.SqlDialects["mysqlconnection"] = new MySqlDialect();
            CommandInterpreterFactory.CommandInterpreters["mysqlconnection"] = d => new MySqlCommandInterpreter(d);

            return option;
        }

        public static IStoreOption UseMySql(
            this IStoreOption option,
            string connectionString)
        {
            return UseMySql(option, connectionString, IsolationLevel.ReadUncommitted);
        }

        public static IStoreOption UseMySql(
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

            RegisterMySql(option);

            option.ConnectionFactory = new DbConnectionFactory<MySqlConnection>(connectionString);
            option.IsolationLevel = isolationLevel;

            return option;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

using Acesoft.Data.Sql;

namespace Acesoft.Data.SqlServer
{
    public static class SqlServerExtensions
    {
        public static IStoreOption RegisterSqlServer(this IStoreOption option)
        {
            SqlDialectFactory.SqlDialects["sqlconnection"] = new SqlServerDialect();
            CommandInterpreterFactory.CommandInterpreters["sqlconnection"] = d => new SqlServerCommandInterpreter(d);

            return option;
        }

        public static IStoreOption UseSqlServer(
            this IStoreOption option,
            string connectionString)
        {
            return UseSqlServer(option, connectionString, IsolationLevel.ReadUncommitted);
        }

        public static IStoreOption UseSqlServer(
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

            RegisterSqlServer(option);
            option.ConnectionFactory = new DbConnectionFactory<SqlConnection>(connectionString);
            option.IsolationLevel = isolationLevel;

            return option;
        }
    }
}

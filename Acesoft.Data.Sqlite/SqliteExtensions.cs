using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

using Microsoft.Data.Sqlite;
using Acesoft.Data.Sql;

namespace Acesoft.Data.Sqlite
{
    public static class SqliteExtensions
    {
        public static IStoreOption RegisterSqLite(this IStoreOption option)
        {
            SqlDialectFactory.SqlDialects["sqliteconnection"] = new SqliteDialect();
            CommandInterpreterFactory.CommandInterpreters["sqliteconnection"] = d => new SqliteCommandInterpreter(d);

            return option;
        }

        public static IStoreOption UseInMemory(this IStoreOption option)
        {
            const string inMemoryConnectionString = "Data Source=:memory:";
            return UseSqLite(option, inMemoryConnectionString, IsolationLevel.Serializable, shareConnection: true);
        }

        public static IStoreOption UseSqLite(
            this IStoreOption option,
            string connectionString)
        {
            return UseSqLite(
                option,
                connectionString,
                IsolationLevel.Serializable);
        }

        public static IStoreOption UseSqLite(
            this IStoreOption option,
            string connectionString,
            IsolationLevel isolationLevel,
            bool shareConnection = false)
        {
            if (option == null)
            {
                throw new ArgumentNullException(nameof(option));
            }

            if (String.IsNullOrWhiteSpace(connectionString))
            {
                throw new ArgumentException(nameof(connectionString));
            }

            RegisterSqLite(option);
            option.ConnectionFactory = new DbConnectionFactory<SqliteConnection>(connectionString, shareConnection);
            option.IsolationLevel = isolationLevel;

            return option;
        }
    }
}

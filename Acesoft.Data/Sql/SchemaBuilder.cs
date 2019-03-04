using System;
using System.Collections.Generic;
using System.Data;

using Dapper;
using Acesoft.Data.Sql.Schema;

namespace Acesoft.Data.Sql
{
    public class SchemaBuilder
    {
        private ICommandInterpreter _builder;
        private string _tablePrefix;
        private ISqlDialect _dialect;
        public ISession Session { get; private set; }
        public bool ThrowOnError { get; set; }

        public SchemaBuilder(ISession session, bool throwOnError = true)
        {
            Session = session;
            ThrowOnError = throwOnError;
            _builder = CommandInterpreterFactory.For(session.Connection);
            _dialect = session.Store.Dialect;
            _tablePrefix = session.Store.Option.TablePrefix;
        }

        private void Execute(IEnumerable<string> statements)
        {
            foreach (var statement in statements)
            {
                Session.Execute(statement, null);
            }
        }

        private string Prefix(string table)
        {
            return _tablePrefix + table;
        }

        public SchemaBuilder CreateTable(string name, Action<CreateTableCommand> table)
        {
            try
            {
                var createTable = new CreateTableCommand(Prefix(name));
                table(createTable);
                Execute(_builder.CreateSql(createTable));
            }
            catch
            {
                if (ThrowOnError)
                {
                    throw;
                }
            }

            return this;
        }

        public SchemaBuilder AlterTable(string name, Action<AlterTableCommand> table)
        {
            try
            {
                var alterTable = new AlterTableCommand(Prefix(name), _dialect, _tablePrefix);
                table(alterTable);
                Execute(_builder.CreateSql(alterTable));
            }
            catch
            {
                if (ThrowOnError)
                {
                    throw;
                }
            }

            return this;
        }

        public SchemaBuilder DropTable(string name)
        {
            try
            {
                var deleteTable = new DropTableCommand(Prefix(name));
                Execute(_builder.CreateSql(deleteTable));
            }
            catch
            {
                if (ThrowOnError)
                {
                    throw;
                }
            }

            return this;
        }

        public SchemaBuilder CreateForeignKey(string name, string srcTable, string srcColumn, string destTable, string destColumn)
        {
            try
            {
                var command = new CreateForeignKeyCommand(Prefix(name), Prefix(srcTable), new string[] { srcColumn }, Prefix(destTable), new string[] { destColumn });
                Execute(_builder.CreateSql(command));
            }
            catch
            {
                if (ThrowOnError)
                {
                    throw;
                }
            }

            return this;
        }

        public SchemaBuilder CreateForeignKey(string name, string srcTable, string[] srcColumns, string destTable, string[] destColumns)
        {
            try
            {
                var command = new CreateForeignKeyCommand(Prefix(name), Prefix(srcTable), srcColumns, Prefix(destTable), destColumns);
                Execute(_builder.CreateSql(command));
            }
            catch
            {
                if (ThrowOnError)
                {
                    throw;
                }
            }

            return this;
        }

        public SchemaBuilder DropForeignKey(string srcTable, string name)
        {
            try
            {
                var command = new DropForeignKeyCommand(Prefix(srcTable), Prefix(name));
                Execute(_builder.CreateSql(command));
            }
            catch
            {
                if (ThrowOnError)
                {
                    throw;
                }
            }

            return this;
        }
    }
}

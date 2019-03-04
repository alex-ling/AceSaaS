using System;
using System.Data;

namespace Acesoft.Data.Sql.Schema
{
    public class CreateTableCommand : SchemaCommand, ICreateTableCommand
    {
        public CreateTableCommand(string name)
            : base(name, SchemaCommandType.CreateTable)
        {
        }

        public ICreateTableCommand PrimaryKey()
        {
            return Column<long>("id", c => c.PrimaryKey());
        }

        public ICreateTableCommand Column(string columnName, DbType dbType, Action<ICreateColumnCommand> column = null)
        {
            var command = new CreateColumnCommand(Name, columnName);
            command.WithType(dbType);

            if (column != null)
            {
                column(command);
            }
            TableCommands.Add(command);
            return this;
        }

        public ICreateTableCommand Column<T>(string columnName, Action<ICreateColumnCommand> column = null)
        {
            var dbType = SchemaUtils.ToDbType(typeof(T));
            return Column(columnName, dbType, column);
        }

    }
}

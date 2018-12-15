using System;
using System.Collections.Generic;

namespace Acesoft.Data.Sql.Schema
{
    public interface ISchemaCommand
    {
        string Name { get; }
        List<ITableCommand> TableCommands { get; }
    }

    public enum SchemaCommandType
    {
        CreateTable,
        DropTable,
        AlterTable,
        SqlStatement,
        CreateForeignKey,
        DropForeignKey
    }
}

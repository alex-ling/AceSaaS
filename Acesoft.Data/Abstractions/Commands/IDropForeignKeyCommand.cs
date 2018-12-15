using System;

namespace Acesoft.Data.Sql.Schema
{
    public interface IDropForeignKeyCommand : ISchemaCommand
    {
        string SrcTable { get; }
    }
}

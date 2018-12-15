using System;

namespace Acesoft.Data.Sql.Schema
{
    public class DropColumnCommand : ColumnCommand, IDropColumnCommand
    {
        public DropColumnCommand(string tableName, string columnName)
            : base(tableName, columnName)
        {
        }
    }
}

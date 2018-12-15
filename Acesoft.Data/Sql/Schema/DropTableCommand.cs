using System;

namespace Acesoft.Data.Sql.Schema
{
    public class DropTableCommand : SchemaCommand, IDropTableCommand
    {
        public DropTableCommand(string name)
            : base(name, SchemaCommandType.DropTable)
        {
        }
    }
}
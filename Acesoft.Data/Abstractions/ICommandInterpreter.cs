using System;
using System.Collections.Generic;
using System.Text;

using Acesoft.Data.Sql.Schema;

namespace Acesoft.Data
{
    public interface ICommandInterpreter
    {
        IEnumerable<string> CreateSql(IEnumerable<ISchemaCommand> commands);
        void Run(StringBuilder builder, IAddColumnCommand command);
        void Run(StringBuilder builder, IDropColumnCommand command);
        void Run(StringBuilder builder, IAlterColumnCommand command);
        void Run(StringBuilder builder, IAddIndexCommand command);
        void Run(StringBuilder builder, IDropIndexCommand command);
        IEnumerable<string> Run(ICreateTableCommand command);
        IEnumerable<string> Run(IDropTableCommand command);
        IEnumerable<string> Run(IAlterTableCommand command);
        IEnumerable<string> Run(ISqlStatementCommand command);
        IEnumerable<string> Run(ICreateForeignKeyCommand command);
        IEnumerable<string> Run(IDropForeignKeyCommand command);
    }
}

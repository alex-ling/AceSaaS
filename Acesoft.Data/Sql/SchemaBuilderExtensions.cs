using System.Collections.Generic;
using System.Text;
using Acesoft.Data.Sql.Schema;

namespace Acesoft.Data.Sql
{
    public static class SchemaBuilderExtensions
    {
        public static IEnumerable<string> CreateSql(this ICommandInterpreter builder, ISchemaCommand command)
        {
            return builder.CreateSql(new[] { command });
        }
    }

}

using System;
using System.Collections.Generic;
using System.Text;

using Acesoft.Data.Sql;

namespace Acesoft.Data.SqlServer
{
    public class SqlServerCommandInterpreter : BaseCommandInterpreter
    {
        public SqlServerCommandInterpreter(ISqlDialect dialect) : base(dialect)
        {
        }
    }
}

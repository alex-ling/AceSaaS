using System;
using System.Collections.Generic;

namespace Acesoft.Data.Sql.Schema
{
    public interface ISqlStatementCommand : ISchemaCommand
    {
        string Sql { get; }
        List<string> Providers { get; }
    }
}

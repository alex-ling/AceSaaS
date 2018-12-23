using System;

namespace Acesoft.Data.Sql.Schema
{
    public interface IAddIndexCommand : ITableCommand
    {
        string IndexName { get; set; }
        string[] ColumnNames { get; }
    }
}

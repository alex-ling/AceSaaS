using System;

namespace Acesoft.Data.Sql.Schema
{
    public interface IDropIndexCommand : ITableCommand
    {
        string IndexName { get; set; }
    }
}

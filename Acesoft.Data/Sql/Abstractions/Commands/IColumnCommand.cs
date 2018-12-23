using System.Data;

namespace Acesoft.Data.Sql.Schema
{
    public interface IColumnCommand : ITableCommand
    {
        string ColumnName { get; set; }
        byte Scale { get; }

        byte Precision { get; }

        DbType DbType { get; }

        object Default { get; }

        int? Length { get; }

        IColumnCommand WithDefault(object @default);

        IColumnCommand WithLength(int? length);

        IColumnCommand Unlimited();
    }
}

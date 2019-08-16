using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Acesoft.Data
{
    /// <summary>
    /// Data Tag is a string with expression: {field|type|format}
    /// </summary>
    public class DataTag : IDataTag
    {
        public DataRow DataRow { get; set; }
        public string Expression { get; set; }
        public int RowIndex { get; set; }

        public DataTag(DataRow dataRow, string expression, int rowIndex)
        {
            this.DataRow = dataRow;
            this.Expression = expression;
            this.RowIndex = rowIndex;
        }

        public string Output()
        {
            var items = Expression.Split('|');
            var field = items[0];
            object value = null;

            if (field.StartsWith("@"))
            {
                if (field == "@index") value = RowIndex;
            }
            else if (DataRow != null && DataRow.Table.Columns.Contains(field))
            {
                value = DataRow[field];
            }

            if (value == null || value == Convert.DBNull)
            {
                return string.Empty;
            }

            var type = items.Length > 1 ? items[1].ToLower() : "text";
            var format = items.Length > 2 ? items[2] : null;

            var rv = string.Empty;
            switch (type)
            {
                case "text":
                    rv = value.ToString().Replace("\r\n", "");
                    break;
                case "num":
                    rv = Convert.ToDouble(value).ToString(format);
                    break;
                case "date":
                    rv = (Convert.ToDateTime(value)).ToString(format ?? "yyyy-MM-dd");
                    break;
                case "bool":
                    rv = Convert.ToBoolean(value) ? format : "";
                    break;
                default:
                    rv = value.ToString();
                    break;
            }

            return rv;
        }
    }
}

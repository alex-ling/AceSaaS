using Acesoft.Util;
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
        public const string Tag_Text = "text";
        public const string Tag_Num = "num";
        public const string Tag_Chs = "chs";
        public const string Tag_Money = "money";
        public const string Tag_Date = "date";
        public const string Tag_Bool = "bool";
        public const string Tag_Attach = "attach";

        public object DataRow { get; set; }
        public string Expression { get; set; }
        public int RowIndex { get; set; }
        public string Field { get; set; }
        public string Type { get; set; }
        public string Format { get; set; }

        public DataTag(object dataRow, string expression, int rowIndex = 1)
        {
            this.DataRow = dataRow;
            this.Expression = expression;
            this.RowIndex = rowIndex;

            var items = Expression.Split('|');
            this.Field = items[0];
            this.Type = items.Length > 1 ? items[1].ToLower() : "text";
            this.Format = items.Length > 2 ? items[2] : null;
        }

        public string Output()
        {
            object value = null;

            if (Field.StartsWith("@"))
            {
                if (Field == "@index") value = RowIndex;
            }
            else if (DataRow != null)
            {
                if (DataRow is DataRow row && row.Table.Columns.Contains(Field))
                {
                    value = row[Field];
                }
                else if (DataRow is IDictionary<string, object> dict && dict.ContainsKey(Field))
                {
                    value = dict[Field];
                }
            }

            if (value == null || value == Convert.DBNull)
            {
                return string.Empty;
            }

            var rv = string.Empty;
            switch (Type)
            {
                case Tag_Text:
                    rv = value.ToString().Replace("\r\n", "");
                    break;
                case Tag_Num:
                    rv = Convert.ToDouble(value).ToString(Format);
                    break;
                case Tag_Date:
                    rv = (Convert.ToDateTime(value)).ToString(Format ?? "yyyy-MM-dd");
                    break;
                case Tag_Chs:
                    rv = ChsHelper.GetChs(value.ToString());
                    break;
                case Tag_Money:
                    rv = ChsHelper.GetChsMoney((decimal)value);
                    break;
                case Tag_Bool:
                    rv = Convert.ToBoolean(value) ? Format : "";
                    break;
                case Tag_Attach:
                    rv = value.ToString().TrimStart(',');
                    break;
                default:
                    rv = value.ToString();
                    break;
            }

            return rv;
        }
    }
}

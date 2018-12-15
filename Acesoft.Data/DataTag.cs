using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Acesoft.Data
{
    public static class DataTag
    {
        public const string REG_Text = @"(?<=\{)([^\;\|\{\}]{0,}\|){0,2}[^\;\|\{\}]{0,}(?=\})";

        public static void Match(object obj, Action<int, string, string> action)
        {
            if (obj == null) return;

            Match(obj.ToString(), action);
        }

        public static void Match(string s, Action<int, string, string> action)
        {
            if (s.HasValue())
            {
                var regexs = Regex.Matches(s, REG_Text, RegexOptions.Compiled);
                for (int i = 0; i < regexs.Count; i++)
                {
                    action(i, regexs[i].Value, regexs[i].Value.Split('|')[0]);
                }
            }
        }

        public static string ReplaceTag(string s, DataRow row, int index)
        {
            Match(s, (ix, expr, field) =>
            {
                s = s.Replace("{" + expr + "}", ToTagStr(row, expr, index));
            });
            return s;
        }

        public static string ToTagStr(DataRow row, string expr, int index)
        {            
            var items = expr.Split('|');
            var field = items[0];
            object value = null;

            if (field.StartsWith("@"))
            {
                if (field == "@index") value = index;
            }
            else if (row != null && row.Table.Columns.Contains(field))
            {
                value = row[field];
            }

            if (value == null || value == Convert.DBNull)
            {
                return string.Empty;
            }

            var rv = string.Empty;
            var type = items.Length > 1 ? items[1].ToLower() : "text";
            var fmt = items.Length > 2 ? items[2] : null;

            switch (type)
            {
                case "text":
                    rv = value.ToString();
                    break;
                case "date":
                    rv = (Convert.ToDateTime(value)).ToString(fmt ?? "yyyy-MM-dd");
                    break;
                default:
                    rv = value.ToString();
                    break;
            }

            return rv;
        }
    }
}

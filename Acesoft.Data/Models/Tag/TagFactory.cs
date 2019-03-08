using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

using Acesoft.Util;

namespace Acesoft.Data
{
    public class TagFactory
    {
        public const string REG_Tag = @"(?<=\{)([^\;\|\{\}]{0,}\|){0,2}[^\;\|\{\}]{0,}(?=\})";

        public string ReplaceTag(string str, DataRow dataRow, int rowIndex)
        {
            if (!str.HasValue())
            {
                return str;
            }

            return RegexHelper.Replace(str, REG_Tag, m => ToTagString(dataRow, m.Value, rowIndex));
        }

        public string ToTagString(DataRow dataRow, string expression, int rowIndex)
        {
            return new DataTag(dataRow, expression, rowIndex).Output();
        }
    }
}

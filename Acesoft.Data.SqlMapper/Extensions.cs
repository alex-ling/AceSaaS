using System;
using System.Collections.Generic;
using System.Text;

using Dapper;

namespace Acesoft.Data.SqlMapper
{
    public static class Extensions
    {
        public static string Replace(this string str, DynamicParameters param)
        {
            foreach (var key in param.ParameterNames)
            {
                str = str.Replace($"{{@{key}}}", (param.Get<object>(key) ?? "").ToString());
            }

            return str;
        }
    }
}

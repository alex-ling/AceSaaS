using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;

using Acesoft.Util;

namespace Acesoft
{
    public static class DictExtensions
    {
        public static T GetValue<T>(this NameValueCollection dict, string key)
        {
            var val = dict[key];
            Check.Require(val.HasValue(), $"字典中不包含查询项{key}");

            return val.ToObject<T>();
        }

        public static T GetValue<T>(this NameValueCollection dict, string key, T defaultValue)
        {
            var val = dict[key];
            if (val.HasValue())
            {
                return val.ToObject<T>();
            }
            return defaultValue;
        }

        public static T GetValue<T>(this IDictionary<string, string> dict, string key)
        {
            Check.Require(dict.ContainsKey(key), $"字典中不包含查询项{key}");

            return dict[key].ToObject<T>();
        }

        public static T GetValue<T>(this IDictionary<string, string> dict, string key, T defaultValue)
        {
            if (!dict.ContainsKey(key) || !dict[key].HasValue())
            {
                return defaultValue;
            }
            return dict[key].ToObject<T>();
        }

        public static T GetValue<T>(this IDictionary<string, object> dict, string key)
        {
            Check.Require(dict.ContainsKey(key), $"字典中不包含查询项{key}");

            return dict[key].ToObject<T>();
        }

        public static T GetValue<T>(this IDictionary<string, object> dict, string key, T defaultValue)
        {
            if (!dict.ContainsKey(key) || dict[key] == null)
            {
                return defaultValue;
            }
            return dict[key].ToObject<T>();
        }

        public static T GetValue<T>(this IDictionary<string, T> dict, string key)
        {
            Check.Require(dict.ContainsKey(key), $"字典中不包含查询项{key}");

            return dict[key];
        }

        public static T GetValue<T>(this IDictionary<string, T> dict, string key, T defaultValue)
        {
            if (!dict.ContainsKey(key) || dict[key] == null)
            {
                return defaultValue;
            }
            return dict[key];
        }
        
        public static IDictionary<string, object> AppendValue(this IDictionary<string, object> dict, string key, object value, string separator = ",")
        {
            dict[key] = !dict.ContainsKey(key) ? value.ToString() : (dict[key] + separator + value);
            return dict;
        }

        public static void PrependValue(this IDictionary<string, object> dict, string key, object value, string separator = ",")
        {
            dict[key] = !dict.ContainsKey(key) ? value.ToString() : (value + separator + dict[key]);
        }

        public static IDictionary<string, object> Append(this IDictionary<string, object> dict, string key, object value, bool replace = true)
        {
            if (replace || !dict.ContainsKey(key))
            {
                dict[key] = value;
            }
            return dict;
        }

        public static IDictionary<T, V> Merge<T, V>(this IDictionary<T, V> dict, IDictionary<T, V> from, bool replace = true)
        {
            foreach (var p in from)
            {
                if (replace || !dict.ContainsKey(p.Key))
                {
                    dict[p.Key] = p.Value;
                }
            }
            return dict;
        }        

        public static IDictionary<string, object> Merge(this IDictionary<string, object> dict, object value, bool replace = true)
        {
            return dict.Merge(ConvertHelper.ObjectToDictionary(value), replace);
        }
    }
}

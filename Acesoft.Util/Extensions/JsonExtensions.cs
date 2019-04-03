using System;
using System.Collections.Generic;
using System.Text;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Acesoft
{
    public static class JsonExtensions
    {
        public static T GetNullValue<T>(this JObject json, string key, T defaultValue = default(T))
        {
            if (json[key] != null)
            {
                return json[key].Value<T>();
            }
            return defaultValue;
        }

        public static T GetValue<T>(this JObject json, string key, T defaultValue = default(T))
        {
            if (json[key] != null && json[key].Value<string>() != "")
            {
                return json[key].Value<T>();
            }
            return defaultValue;
        }

        public static IDictionary<string, object> ToDictionary(this JObject json)
        {
            var dict = json.ToObject<Dictionary<string, object>>();
            dict.Remove("__RequestVerificationToken");
            return dict;
        }

        public static void WriteDbValue(this JsonWriter writer, object val)
        {
            object obj;
            if (val == Convert.DBNull)
            {
                writer.WriteNull();
            }
            else if ((obj = val) is long)
            {
                writer.WriteValue(((long)obj).ToString());
            }
            else if ((obj = val) is bool)
            {
                writer.WriteValue(((bool)obj) ? 1 : 0);
            }
            else
            {
                writer.WriteValue(val);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Acesoft
{
    public static class HttpContextExtensions
    {
        public static string GetServerIp(this HttpContext context)
        {
            return context?.Connection.LocalIpAddress.ToString();
        }

        public static string GetClientIp(this HttpContext context)
        {
            return context?.Connection.RemoteIpAddress.ToString();
        }

        public static string GetInitScripts(this HttpContext context)
        {
            if (context.Items.TryGetValue(App.Script_Init, out object val))
            {
                return val.ToString();
            }
            return "";
        }

        public static void AppendInitScripts(this HttpContext context, string value)
        {
            if (context.Items.TryGetValue(App.Script_Init, out object val))
            {
                value = val.ToString() + value;
            }
            context.Items[App.Script_Init] = value;
        }

        public static T GetOrAddScoped<T>(this HttpContext context, string key, Func<string, T> valueFactory)
        {
            if (context.Items.TryGetValue(key, out object value))
            {
                return (T)value;
            }

            var newValue = valueFactory(key);
            context.Items[key] = newValue;
            return newValue;
        }

        public static T GetOrAddSession<T>(this HttpContext context, string key, Func<string, T> valueFactory)
        {
            var str = context.Session.GetString(key);
            if (str == null)
            {
                var value = valueFactory(key);
                str = JsonConvert.SerializeObject(value);
                context.Session.SetString(key, str);
                return value;
            }
            return JsonConvert.DeserializeObject<T>(str);
        }
    }
}

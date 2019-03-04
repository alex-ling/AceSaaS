using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Acesoft
{
    public static class Extensions
    {
        #region http
        public static string GetServerIp(this HttpContext context)
        {
            return context?.Connection.LocalIpAddress.ToString();
        }

        public static string GetClientIp(this HttpContext context)
        {
            return context?.Connection.RemoteIpAddress.ToString();
        }

        public static string GetUrl(this HttpRequest req)
        {
            return $"{req.Scheme}://{req.Host}{req.PathBase}{req.Path}{req.QueryString}";
        }

        public static string GetPath(this HttpRequest req)
        {
            return $"{req.PathBase}{req.Path}";
        }

        public static string GetAppName(this HttpRequest req)
        {
            string[] array = req.Path.Value.Split('/');
            if (array.Length <= 1)
            {
                return "";
            }
            return array[1];
        }

        public static string GetModName(this HttpRequest req)
        {
            string[] array = req.Path.Value.Split('/');
            if (array.Length <= 2)
            {
                return "";
            }
            return array[2];
        }

        public static string WebRoot(this HttpRequest req)
        {
            return $"{req.Scheme}://{req.Host}/";
        }
        #endregion

        #region scope
        public static string GetInitScripts(this HttpContext context)
        {
            if (context.Items.TryGetValue("Script_Init", out object val))
            {
                return val.ToString();
            }
            return null;
        }

        public static string AppendInitScripts(this HttpContext context, string value)
        {
            if (context.Items.TryGetValue("Script_Init", out object val))
            {
                value = val.ToString() + value;
            }
            return value;
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
        #endregion

        #region session
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
        #endregion
    }
}

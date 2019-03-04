using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace Acesoft.Util.Helper
{
    public static class UrlHelper
    {
        public static T GetQuery<T>(string url, string name)
        {
            var query = HttpUtility.ParseQueryString(url);
            var value = query[name];
            if (value.HasValue())
            {
                return value.ToObject<T>();
            }
            throw new AceException($"Cannot get query name with \"{name}\" from url: {url}.");
        }

        public static T GetQuery<T>(string url, string name, T defaultValue)
        {
            var query = HttpUtility.ParseQueryString(url);
            var value = query[name];
            if (value.HasValue())
            {
                return value.ToObject<T>();
            }
            return defaultValue;
        }

        public static string Append(string url, string name, string value)
        {
            var query = $"{name}={value}";
            var items = url.Split('#');
            var str = items[0];
            var hash = (items.Length > 1) ? items[1] : "";

            if (str.IndexOf("?") > 0)
            {
                var st = str.IndexOf(name + "=");
                if (st > 0)
                {
                    var ed = str.IndexOf("&", st);
                    str = (ed <= 0) ? (str.Substring(0, st) + query) : (str.Substring(0, st) + query + str.Substring(ed));
                }
                else
                {
                    str += "&" + query;
                }
            }
            else
            {
                str += "?" + query;
            }

            if (hash.HasValue())
            {
                str += "#" + hash;
            }
            return str;
        }
    }
}

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
    }
}

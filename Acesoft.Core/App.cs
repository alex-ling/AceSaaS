using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Acesoft.Core
{
    public static class App
    {
        private static IHttpContextAccessor httpContextAccessor;

        public static HttpContext Context => httpContextAccessor?.HttpContext;

        public static IServiceProvider UseAppContext(this IServiceProvider service)
        {
            httpContextAccessor = service.GetService<IHttpContextAccessor>();

            return service;
        }

        #region path
        public static string GetWebPath()
        {
            return "";
        }

        public static string GetLocalPath(string virtualPath, bool mustCreatePath = false)
        {
            if (virtualPath.HasValue())
            {
                var path = Path.Combine(AppContext.BaseDirectory, virtualPath);
                if (mustCreatePath && !Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                return path;
            }
            return AppContext.BaseDirectory;
        }
        #endregion

        #region query
        public static T GetQuery<T>(string name)
        {
            if (Context != null)
            {
                var query = Context.Request.Query[name];
                if (query.Count > 0)
                {
                    return query[0].ToObject<T>();
                }
            }
            throw new AceException($"Cannot get query name with [{name}] from request url.");
        }

        public static T GetParam<T>(string name, T defaultValue)
        {
            var post = Context?.Request.Method == "POST";
            return post ? GetForm<T>(name, defaultValue) : GetQuery<T>(name, defaultValue);
        }

        public static T GetQuery<T>(string name, T defaultValue)
        {
            if (Context != null)
            {
                var query = Context.Request.Query[name];
                if (query.Count > 0)
                {
                    return query[0].ToObject<T>();
                }
            }
            return defaultValue;
        }

        public static T GetForm<T>(string name, T defaultValue)
        {
            if (Context != null)
            {
                var query = Context.Request.Form[name];
                if (query.Count > 0)
                {
                    return query[0].ToObject<T>();
                }
            }
            return defaultValue;
        }

        public static string ReplaceQuery(string str)
        {
            if (Context != null)
            {
                foreach (var q in Context.Request.Query)
                {
                    str = str.Replace($"{{@{q.Key}}}", q.Value);
                }
            }

            return str;
        }
        #endregion
    }
}

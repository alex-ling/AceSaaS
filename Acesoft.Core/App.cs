using System;
using System.IO;
using System.Text.Encodings.Web;
using System.Text.Unicode;

using Acesoft.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Acesoft
{
    public static class App
    {
        private static IHttpContextAccessor httpContextAccessor;

        public static HtmlEncoder DefaultEncoder => HtmlEncoder.Create(UnicodeRanges.All);
        public static HttpContext Context => httpContextAccessor?.HttpContext;
        public static IIdWorker IdWorker { get; private set; }

        public static IServiceProvider UseAppContext(this IServiceProvider service)
        {
            httpContextAccessor = service.GetService<IHttpContextAccessor>();
            IdWorker = service.GetService<IIdWorker>();

            return service;
        }        

        #region path
        public static string GetWebRoot(bool fullPath = false)
        {
            if (fullPath)
            {
                return Context.Request.WebRoot();
            }
            return "/";
        }

        public static string GetWebPath(string path, bool fullPath = false)
        {
            path = path.TrimStart(',', '~', '/');
            if (path.StartsWith("http"))
            {
                return path;
            }
            return GetWebRoot(fullPath) + path;
        }

        public static string GetWebPhoto(string path, bool fullPath = false, string none = "images/none.jpg")
        {
            if (path.HasValue())
            {
                return GetWebPath(path, fullPath);
            }
            return GetWebPath(none, fullPath);
        }

        public static string GetLocalRoot()
        {
            return AppContext.BaseDirectory;
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
            return GetLocalRoot();
        }

        public static string GetLocalResource(string path)
        {
            if (path.StartsWith("~"))
            {
                path = path.Substring(1);
            }
            path = path.TrimStart(',', '/');
            return Path.Combine(GetLocalRoot(), "wwwroot/" + path);
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

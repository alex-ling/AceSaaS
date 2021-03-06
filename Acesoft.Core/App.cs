﻿using System;
using System.IO;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Unicode;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Acesoft.Config;
using Acesoft.Core;
using Microsoft.AspNetCore.Authentication;

namespace Acesoft
{
    public static class App
    {
        public const string Script_Init = "Script_Init";

        private static IHttpContextAccessor httpContextAccessor;
        private static AppConfig appConfig;
        public static AppConfig AppConfig
        {
            get
            {
                if (appConfig == null)
                {
                    appConfig = ConfigContext.GetConfig<AppConfig>("", (cfg, key) =>
                    {
                        // refresh object while changed.
                        appConfig = cfg;
                    });
                }
                return appConfig;
            }
        }

        public static HtmlEncoder DefaultEncoder => HtmlEncoder.Create(UnicodeRanges.All);
        public static HttpContext Context => httpContextAccessor?.HttpContext;
        public static IIdWorker IdWorker { get; private set; }

        public static IMemoryCache MemoryCache { get; private set; }
        public static IDistributedCache Cache { get; private set; }        

        public static IServiceProvider UseAppContext(this IServiceProvider service)
        {
            // For utf-8
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            // Global HttpContextWarpper
            httpContextAccessor = service.GetService<IHttpContextAccessor>();

            var httpService = Context?.RequestServices ?? service;
            IdWorker = httpService.GetService<IIdWorker>();
            MemoryCache = httpService.GetService<IMemoryCache>();
            Cache = httpService.GetService<IDistributedCache>();

            return service;
        }

        public static void SetAppConfig(AppConfig appConfig = null)
        {
            App.appConfig = appConfig;
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

        public static string GetLocalBasePath(string path)
        {
            if (Path.IsPathRooted(path))
            {
                return path.Substring(GetLocalRoot().Length);
            }
            return path;
        }

        public static string GetLocalPath(string virtualPath, bool mustCreatePath = false)
        {
            if (virtualPath.HasValue())
            {
                var path = Path.Combine(AppContext.BaseDirectory, virtualPath.TrimStart('/'));
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
                if (query.Count > 0 && query[0].HasValue())
                {
                    return query[0].ToObject<T>();
                }
            }
            throw new AceException($"Cannot get query value with [{name}]");
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
                if (query.Count > 0 && query[0].HasValue())
                {
                    return query[0].ToObject<T>();
                }
            }
            return defaultValue;
        }

        public static T GetForm<T>(string name)
        {
            if (Context != null)
            {
                var query = Context.Request.Form[name];
                if (query.Count > 0 && query[0].HasValue())
                {
                    return query[0].ToObject<T>();
                }
            }
            throw new AceException($"Cannot get form value with [{name}]");
        }

        public static T GetForm<T>(string name, T defaultValue)
        {
            if (Context != null)
            {
                var query = Context.Request.Form[name];
                if (query.Count > 0 && query[0].HasValue())
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

        #region cookie
        public static T GetCookie<T>(string name, T defaultValue)
        {
            if (Context != null)
            {
                string val = Context.Request.Cookies[name];
                if (val.HasValue())
                {
                    return val.ToObject<T>();
                }
            }
            return defaultValue;
        }
        #endregion
    }
}

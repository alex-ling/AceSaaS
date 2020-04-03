using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Acesoft
{
    public static class HttpRequestExtensions
    {
        public static string GetUrl(this HttpRequest req, bool pathOnly = false)
        {
            if (!pathOnly)
            {
                return $"{req.Scheme}://{req.Host}{req.PathBase}{req.Path}{req.QueryString}";
            }
            return $"{req.PathBase}{req.Path}{req.QueryString}";
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

        public static string GetScheme(this HttpRequest req)
        {
            return req.Scheme;
        }

        public static string WebRoot(this HttpRequest req)
        {
            return $"{req.Scheme}://{req.Host}/";
        }
    }
}

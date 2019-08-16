using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Extensions.Caching.Distributed;
using Acesoft.Util;

namespace Acesoft.Cache
{
    public static class CacheExtensions
    {
        public static T GetOrAdd<T>(this IDistributedCache cache, string key, Func<string, T> addFunc)
        {
            var result = cache.Get<T>(key);
            if (result == null)
            {
                result = addFunc(key);
                cache.Set(key, result);
            }
            return result;
        }

        public static string GetOrAdd(this IDistributedCache cache, string key, Func<string, string> addFunc)
        {
            var result = cache.GetString(key);
            if (result == null)
            {
                result = addFunc(key);
                cache.SetString(key, result);
            }
            return result;
        }

        public static T Get<T>(this IDistributedCache cache, string key)
        {
            var json = cache.GetString(key);
            if (json != null)
            {
                return SerializeHelper.FromJson<T>(json);
            }
            return default(T);
        }

        public static void Set(this IDistributedCache cache, string key, object obj, Action<DistributedCacheEntryOptions> options = null)
        {
            var json = SerializeHelper.ToJson(obj);
            var option = new DistributedCacheEntryOptions();
            options?.Invoke(option);
            cache.SetString(key, json, option);
        }

        public static T GetBinary<T>(this IDistributedCache cache, string key)
        {
            var bytes = cache.Get(key);
            if (bytes != null)
            {
                return SerializeHelper.FromBinary<T>(bytes);
            }
            return default(T);
        }

        public static void SetBinary(this IDistributedCache cache, string key, object obj, Action<DistributedCacheEntryOptions> options = null)
        {
            var bytes = SerializeHelper.ToBinary(obj);
            var option = new DistributedCacheEntryOptions();
            options?.Invoke(option);
            cache.Set(key, bytes, option);
        }

        public static string GetString(this IDistributedCache cache, string key)
        {
            return DistributedCacheExtensions.GetString(cache, key);
        }

        public static void SetString(this IDistributedCache cache, string key, string value, Action<DistributedCacheEntryOptions> options = null)
        {
            var option = new DistributedCacheEntryOptions();
            options?.Invoke(option);
            cache.SetString(key, value, option);
        }
    }
}

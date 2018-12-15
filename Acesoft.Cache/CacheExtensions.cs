using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Extensions.Caching.Distributed;
using Acesoft.Util;

namespace Acesoft.Cache
{
    public static class CacheExtensions
    {
        public static T GetBinary<T>(this IDistributedCache cache, string key)
        {
            var bytes = cache.Get(key);
            return SerializeHelper.FromBinary<T>(bytes);
        }

        public static T GetJson<T>(this IDistributedCache cache, string key)
        {
            var bytes = cache.Get(key);
            return SerializeHelper.FromJson<T>(bytes);
        }

        public static void SetJson(this IDistributedCache cache, string key, object obj, Action<DistributedCacheEntryOptions> options = null)
        {
            var bytes = SerializeHelper.ToJsonBytes(obj);
            var option = new DistributedCacheEntryOptions();
            options?.Invoke(option);
            cache.Set(key, bytes, option);
        }

        public static void SetBinary(this IDistributedCache cache, string key, object obj, Action<DistributedCacheEntryOptions> options = null)
        {
            var bytes = SerializeHelper.ToBinary(obj);
            var option = new DistributedCacheEntryOptions();
            options?.Invoke(option);
            cache.Set(key, bytes, option);
        }

        public static void SetString(this IDistributedCache cache, string key, string value, Action<DistributedCacheEntryOptions> options = null)
        {
            var option = new DistributedCacheEntryOptions();
            options?.Invoke(option);
            cache.SetString(key, value, option);
        }
    }
}

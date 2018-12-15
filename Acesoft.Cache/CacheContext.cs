using System;

using Microsoft.Extensions.Options;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;

namespace Acesoft.Cache
{
    public static class CacheContext
    {
        public static IMemoryCache MemoryCache { get; private set; }
        public static IDistributedCache Cache { get; private set; }

        public static void UseCacheContext(this IServiceProvider serviceProvider)
        {
            MemoryCache = serviceProvider.GetService<IMemoryCache>();
            Cache = serviceProvider.GetService<IDistributedCache>();
        }
    }
}

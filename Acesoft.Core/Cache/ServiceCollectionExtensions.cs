using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Redis;
using Acesoft.Config;
using CSRedis;

namespace Acesoft.Cache
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDistributedRedisCache(this IServiceCollection services)
        {
            // 读取缓存配置文件
            var cacheConfig = ConfigContext.GetJsonConfig<CacheConfig>(opts =>
            {
                opts.Optional = true;
                opts.ConfigFile = "cache.config.json";
            });

            // 添加分布式缓存配置文件
            if (cacheConfig.EnabledDistributedCache)
            {
                services.AddSingleton<IDistributedCache>(sp =>
                {
                    // https://github.com/2881099/Microsoft.Extensions.Caching.CSRedis
                    CSRedisClient csClient;
                    if (!cacheConfig.EnabledCluster)
                    {
                        csClient = new CSRedisClient(cacheConfig.ConnectionString);
                    }
                    else
                    {
                        csClient = new CSRedisClient(connectionString: null, sentinels: cacheConfig.ConnectionStrings);
                    }
                    return new CSRedisCache(csClient);
                });
            }
            else
            {
                services.AddDistributedMemoryCache();
            }

            return services;
        }
    }
}

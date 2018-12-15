using System;
using System.Collections.Generic;
using System.Text;
using Acesoft.Config;
using Microsoft.Extensions.DependencyInjection;

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
            if (cacheConfig.EnabledDistributedRedisCache)
            {
                services.AddDistributedRedisCache(opts =>
                {
                    opts.Configuration = cacheConfig.RedisCacheServer;
                    opts.InstanceName = cacheConfig.RedisCacheInstance;
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

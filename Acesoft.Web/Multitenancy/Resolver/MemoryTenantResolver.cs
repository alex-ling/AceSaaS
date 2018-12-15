using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;

namespace Acesoft.Web.Multitenancy
{
    public abstract class MemoryTenantResolver<T> : ITenantResolver<T>
    {
        protected readonly IMemoryCache cache;
        protected readonly ILogger logger;
        protected readonly MemoryTenantResolverOptions options;

        public TenantsConfig TenantsConfig { get; private set; }

        public MemoryTenantResolver(IMemoryCache cache, 
            TenantsConfig tenantsConfig, ILogger<MemoryTenantResolver<T>> logger)
            : this(cache, tenantsConfig, logger, new MemoryTenantResolverOptions())
        {
        }

        public MemoryTenantResolver(IMemoryCache cache, TenantsConfig tenantsConfig,
            ILogger<MemoryTenantResolver<T>> logger, MemoryTenantResolverOptions options)
        {
            this.cache = cache;
            this.TenantsConfig = tenantsConfig;
            this.logger = logger;
            this.options = options;
        }

        protected abstract string GetContextIdentifier(HttpContext context);
        protected abstract IEnumerable<string> GetTenantIdentifiers(TenantContext<T> context);
        protected abstract Task<TenantContext<T>> ResolveAsync(HttpContext context);

        protected virtual MemoryCacheEntryOptions CreateCacheEntryOptions()
        {
            return new MemoryCacheEntryOptions()
                .SetSlidingExpiration(new TimeSpan(1, 0, 0));
        }

        protected virtual void DisposeTenantContext(object cacheKey, TenantContext<T> tenantContext)
        {
            if (tenantContext != null)
            {
                logger.LogDebug("Disposing TenantContext:{id} instance with key \"{cacheKey}\".", tenantContext.Id, cacheKey);
                tenantContext.Dispose();
            }
        }

        async Task<TenantContext<T>> ITenantResolver<T>.ResolveAsync(HttpContext context)
        {
            // Obtain the key used to identify cached tenants from the current request
            var cacheKey = GetContextIdentifier(context);
            if (cacheKey == null)
            {
                return null;
            }

            var tenantContext = cache.Get(cacheKey) as TenantContext<T>;
            if (tenantContext == null)
            {
                logger.LogDebug("TenantContext not present in cache with key \"{cacheKey}\". Attempting to resolve.", cacheKey);
                tenantContext = await ResolveAsync(context);

                if (tenantContext != null)
                {
                    var tenantIdentifiers = GetTenantIdentifiers(tenantContext);
                    if (tenantIdentifiers != null)
                    {
                        var cacheEntryOptions = GetCacheEntryOptions();

                        logger.LogDebug("TenantContext:{id} resolved. Caching with keys \"{tenantIdentifiers}\".", tenantContext.Id, tenantIdentifiers);

                        foreach (var identifier in tenantIdentifiers)
                        {
                            cache.Set(identifier, tenantContext, cacheEntryOptions);
                        }
                    }
                }
            }
            else
            {
                logger.LogDebug("TenantContext:{id} retrieved from cache with key \"{cacheKey}\".", tenantContext.Id, cacheKey);
            }

            return tenantContext;
        }

        private MemoryCacheEntryOptions GetCacheEntryOptions()
        {
            var cacheEntryOptions = CreateCacheEntryOptions();
            if (options.EvictAllEntriesOnExpire)
            {
                var tokenSource = new CancellationTokenSource();

                cacheEntryOptions
                    .RegisterPostEvictionCallback(
                        (key, value, reason, state) =>
                        {
                            tokenSource.Cancel();
                        })
                    .AddExpirationToken(new CancellationChangeToken(tokenSource.Token));
            }

            if (options.DisposeOnEviction)
            {
                cacheEntryOptions
                    .RegisterPostEvictionCallback(
                        (key, value, reason, state) =>
                        {
                            DisposeTenantContext(key, value as TenantContext<T>);
                        });
            }

            return cacheEntryOptions;
        }
    }
}

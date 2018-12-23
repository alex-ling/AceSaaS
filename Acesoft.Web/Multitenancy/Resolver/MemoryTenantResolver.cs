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
    public abstract class MemoryTenantResolver : ITenantResolver
    {
        protected readonly IMemoryCache cache;
        protected readonly ILogger logger;
        protected readonly MemoryTenantResolverOptions options;

        public MemoryTenantResolver(IMemoryCache cache,
            ILogger<MemoryTenantResolver> logger)
            : this(cache, logger, new MemoryTenantResolverOptions())
        {
        }

        public MemoryTenantResolver(IMemoryCache cache,
            ILogger<MemoryTenantResolver> logger, MemoryTenantResolverOptions options)
        {
            this.cache = cache;
            this.logger = logger;
            this.options = options;
        }

        protected abstract string GetContextIdentifier(HttpContext context);
        protected abstract IEnumerable<string> GetTenantIdentifiers(Tenant tenant);
        protected abstract Task<Tenant> ResolveAsync(HttpContext context);

        protected virtual MemoryCacheEntryOptions CreateCacheEntryOptions()
        {
            return new MemoryCacheEntryOptions()
                .SetSlidingExpiration(new TimeSpan(1, 0, 0));
        }

        protected virtual void DisposeTenantContext(object cacheKey, TenantContext tenantContext)
        {
            if (tenantContext != null)
            {
                logger.LogDebug("Disposing TenantContext:{id} instance with key \"{cacheKey}\".", tenantContext.Id, cacheKey);
                tenantContext.Dispose();
            }
        }

        async Task<Tenant> ITenantResolver.ResolveAsync(HttpContext context)
        {
            // Obtain the key used to identify cached tenants from the current request
            var cacheKey = GetContextIdentifier(context);
            if (cacheKey == null)
            {
                return null;
            }

            var tenant = cache.Get(cacheKey) as Tenant;
            if (tenant == null)
            {
                logger.LogDebug("Tenant not present in cache with key \"{cacheKey}\". Attempting to resolve.", cacheKey);
                tenant = await ResolveAsync(context);

                if (tenant != null)
                {
                    var tenantIdentifiers = GetTenantIdentifiers(tenant);
                    if (tenantIdentifiers != null)
                    {
                        var cacheEntryOptions = GetCacheEntryOptions();

                        logger.LogDebug($"Tenant \"{tenant.Name}\" resolved. Caching with keys \"{tenantIdentifiers.Join()}\".");

                        foreach (var identifier in tenantIdentifiers)
                        {
                            cache.Set(identifier, tenant, cacheEntryOptions);
                        }
                    }
                }
            }
            else
            {
                logger.LogDebug($"Tenant \"{tenant.Name}\" retrieved from cache with key \"{cacheKey}\".");
            }

            return tenant;
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
                            DisposeTenantContext(key, value as TenantContext);
                        });
            }

            return cacheEntryOptions;
        }
    }
}

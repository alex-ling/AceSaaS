using System;
using System.Collections.Generic;
using System.Text;

namespace Acesoft.Web.Multitenancy
{
    public class MemoryTenantResolverOptions
    {
        public MemoryTenantResolverOptions()
        {
            EvictAllEntriesOnExpire = true;
            DisposeOnEviction = true;
        }

        /// <summary>
        /// Gets or sets a setting that determines whether all cache entries for a <see cref="TenantContext{T}"/> 
        /// instance should be evicted when any of the entries expire. Default: True.
        /// </summary>
        public bool EvictAllEntriesOnExpire { get; set; }

        /// <summary>
        /// Gets or sets a setting that determines whether cached tenant context instances should be disposed 
        /// when upon eviction from the cache. Default: True.
        /// </summary>
        public bool DisposeOnEviction { get; set; }
    }
}

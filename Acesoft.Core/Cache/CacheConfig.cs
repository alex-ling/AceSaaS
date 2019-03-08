using System;
using System.Collections.Generic;
using System.Text;

namespace Acesoft.Cache
{
    public class CacheConfig
    {
        public bool EnabledDistributedRedisCache { get; set; }
        public string RedisCacheServer { get; set; }
        public string RedisCacheInstance { get; set; }
    }
}

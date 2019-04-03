using System;
using System.Collections.Generic;
using System.Text;

namespace Acesoft.Cache
{
    public class CacheConfig
    {
        public bool EnabledDistributedCache { get; set; }
        public string ConnectionString { get; set; }

        public bool EnabledCluster { get; set; }
        public string[] ConnectionStrings { get; set; }
    }
}

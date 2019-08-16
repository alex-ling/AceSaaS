using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Redis;
using CSRedis;
using Acesoft.Util;

namespace Acesoft.IotNet
{
    public static class App
    {
        private static IDistributedCache cache;
        public static IDistributedCache Cache
        {
            get
            {
                if (cache == null)
                {
                    // https://github.com/2881099/Microsoft.Extensions.Caching.CSRedis
                    CSRedisClient csClient;
                    if (!ConfigHelper.GetAppSetting("cache:enabledcluster", false))
                    {
                        csClient = new CSRedisClient(ConfigHelper.GetAppSetting<string>("cache:connectionstring"));
                    }
                    else
                    {
                        csClient = new CSRedisClient(connectionString: null, sentinels: 
                            ConfigHelper.GetAppSetting<string>("cache:connectionstrings").Split(';'));
                    }
                    cache = new CSRedisCache(csClient);
                }

                return cache;
            }
        }
    }
}

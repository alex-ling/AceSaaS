using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Acesoft.Data.SqlMapper.Caching
{
    /// <summary>
    /// Least Recently Used
    /// </summary>
    public class LruCacheProvider : ICacheProvider
    {
        private int cacheSize = 0;
        private Hashtable cache = null;
        private IList keyList = null;

        public LruCacheProvider()
        {
            cacheSize = 100;
            cache = Hashtable.Synchronized(new Hashtable());
            keyList = ArrayList.Synchronized(new ArrayList());
        }

        public bool Remove(CacheKey cacheKey)
        {
            object o = this[cacheKey];

            keyList.Remove(cacheKey);
            cache.Remove(cacheKey);
            return true;
        }

        public void Flush()
        {
            cache.Clear();
            keyList.Clear();
        }
        public object this[CacheKey cacheKey]
        {
            get
            {
                keyList.Remove(cacheKey);
                keyList.Add(cacheKey);
                return cache[cacheKey];
            }
            set
            {
                cache[cacheKey] = value;
                keyList.Add(cacheKey);
                if (keyList.Count > cacheSize)
                {
                    object oldestKey = keyList[0];
                    keyList.RemoveAt(0);
                    cache.Remove(oldestKey);
                }
            }
        }
        public void Initialize(IDictionary<string, string> props)
        {
            cacheSize = props.GetValue("cachesize", 1000);
        }
    }
}

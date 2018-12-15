using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Acesoft.Data.SqlMapper.Caching
{
    public class NoneCacheProvider : ICacheProvider
    {
        public object this[CacheKey key]
        {
            get { return null; }
            set { }
        }

        public void Flush()
        {

        }

        public void Initialize(IDictionary<string, string> props)
        {

        }

        public bool Remove(CacheKey key)
        {
            return true;
        }
    }
}

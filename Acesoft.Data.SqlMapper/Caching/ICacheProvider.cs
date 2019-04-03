using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Acesoft.Data.SqlMapper.Caching
{
    public interface ICacheProvider
    {
        void Initialize(IDictionary<string, string> props);
        object this[CacheKey key] { get; set; }
        bool Remove(CacheKey key);
        void Flush();
    }
}

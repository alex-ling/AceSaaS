using System;
using System.Collections.Generic;
using System.Text;

namespace Acesoft.Data.SqlMapper.Caching
{
    public interface ICacheManager
    {
        object this[RequestContext context] { get; set; }
        void Flush(string sqlId);
        void ResetMappedCaches();
    }
}

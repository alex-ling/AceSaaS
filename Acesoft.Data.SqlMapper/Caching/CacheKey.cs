using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using static Dapper.SqlMapper;

namespace Acesoft.Data.SqlMapper.Caching
{
    public class CacheKey
    {
        public RequestContext RequestContext { get; private set; }
        public string QueryString { get; private set; }
        public string Key
        {
            get
            {
                return $"{RequestContext.Scope}.{RequestContext.SqlId}:{QueryString}";
            }
        }

        public string BuildQueryString()
        {
            if (RequestContext.DapperParams == null)
            {
                return "Null";
            }

            var sb = new StringBuilder();
            foreach (var param in RequestContext.Params)
            {
                BuildQueryString(sb, param.Key, param.Value);
            }
            return sb.ToString().Trim('&');
        }

        private void BuildQueryString(StringBuilder sb, string key, object val)
        {
            if (val is IEnumerable list && !(val is String))
            {
                sb.AppendFormat("&{0}=[{1}]", key, list.Join());
            }
            else
            {
                sb.AppendFormat("&{0}={1}", key, val);
            }
        }

        public CacheKey(RequestContext context)
        {
            RequestContext = context;
            QueryString = BuildQueryString();
        }

        public override string ToString() => Key;
        public override int GetHashCode() => Key.GetHashCode();
        public override bool Equals(object obj)
        {
            if (this == obj) return true;
            if (!(obj is CacheKey)) return false;
            CacheKey cacheKey = (CacheKey)obj;
            return cacheKey.Key == Key;
        }
    }
}

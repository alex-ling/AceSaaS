using System;
using System.Collections.Generic;
using System.Collections.Concurrent;

using Microsoft.Extensions.Logging;
using Acesoft.NetCore.Logging;

namespace Acesoft.Data.SqlMapper.Caching
{
    public class CacheManager : ICacheManager
    {
        readonly ILogger logger;

        // #MDY#.ON 2018-03-16 简化缓存失效机制，由存储SqlMap改为存储Cache
        //readonly ConcurrentDictionary<string, IList<SqlMap>> flushSqlMaps = new ConcurrentDictionary<string, IList<SqlMap>>();
        readonly ConcurrentDictionary<string, IList<Cache>> flushSqlMaps = new ConcurrentDictionary<string, IList<Cache>>();
        readonly ConcurrentDictionary<string, DateTime> mappedTimes = new ConcurrentDictionary<string, DateTime>();

        public ISqlMapper SqlMapper { get; private set; }

        #region index
        public object this[RequestContext context]
        {
            get
            {
                var sqlMap = SqlMapper.GetSqlMap(context);
                if (sqlMap.Cache == null)
                {
                    // 未定义缓存时，直接返回null
                    return null;
                }

                if (sqlMap.Cache.FlushInterval.TotalMinutes > 0)
                {
                    lock (this)
                    {
                        FlushByInterval(sqlMap);
                    }
                }

                var cacheKey = new CacheKey(context);
                var cache = sqlMap.Cache.Provider[cacheKey];
                logger.LogDebug($"CacheManager GetCache SqlId: {cacheKey.Key}, Success: {cache != null} !");

                return cache;
            }
            set
            {
                // 清除被影响的缓存，flushonexecute
                Flush(context);

                var sqlMap = SqlMapper.GetSqlMap(context);
                if (sqlMap.Cache == null)
                {
                    // 未定义缓存时，直接返回
                    return;
                }

                if (sqlMap.Cache.FlushInterval.TotalMinutes > 0)
                {
                    lock (this)
                    {
                        FlushByInterval(sqlMap);
                    }
                }

                var cacheKey = new CacheKey(context);
                sqlMap.Cache.Provider[cacheKey] = value;
                logger.LogDebug($"CacheManager SetCache SqlId: {cacheKey.Key}");
            }
        }
        #endregion

        #region ctor
        public CacheManager(ISqlMapper sqlMapper)
        {
            logger = LoggerContext.GetLogger<CacheManager>();
            SqlMapper = sqlMapper;

            this.LoadFlushSqlMaps();
        }

        private void LoadFlushSqlMaps()
        {
            logger.LogDebug($"CacheManager load FlushSqlMaps!");

            foreach (var scope in SqlMapper.MappedScopes.Values)
            {
                // #MDY#.ON 2018-03-16 简化缓存失效机制，由存储SqlMap改为存储Cache
                foreach (var cache in scope.Caches.Values)
                {
                    if (cache.FlushOnExecutes != null)
                    {
                        foreach (var sqlId in cache.FlushOnExecutes)
                        {
                            // 将当前sqlmap加入到需要flush的cache中去
                            flushSqlMaps.GetOrAdd($"{scope.Id}.{sqlId}", new List<Cache>()).Add(cache);
                        }
                    }
                }

                /*foreach (var sqlMap in scope.SqlMaps.Values)
                {
                    if (sqlMap.Cache == null) { continue; }
                    if (sqlMap.Cache.FlushOnExecutes == null) { continue; }

                    foreach (var sqlId in sqlMap.Cache.FlushOnExecutes)
                    {
                        // 将当前sqlmap加入到需要flush的sqlmap中去
                        flushSqlMaps.GetOrAdd($"{scope.Id}.{sqlId}", new List<SqlMap>()).Add(sqlMap);
                    }
                }*/
            }
        }
        #endregion

        #region reset
        public void ResetMappedCaches()
        {
            logger.LogDebug($"CacheManager reset FlushSqlMaps!");

            lock (this)
            {
                flushSqlMaps.Clear();
                LoadFlushSqlMaps();
            }
        }
        #endregion

        #region flush
        public void Flush(string sqlId)
        {
            if (flushSqlMaps.ContainsKey(sqlId))
            {
                lock (this)
                {
                    // #MDY#.ON 2018-03-16 简化缓存失效机制，由存储SqlMap改为存储Cache
                    foreach (var cache in flushSqlMaps[sqlId])
                    {
                        logger.LogDebug($"CacheManager FlushCache.OnExecute CacheId: {cache.Id}, ExeSqlId: {sqlId}");

                        cache.Provider.Flush();
                        mappedTimes[cache.Id] = DateTime.Now;
                    }

                    /*foreach (var sqlMap in flushSqlMaps[sqlId])
                    {
                        logger.LogDebug($"CacheManager FlushCache.OnExecute SqlId: {sqlMap.SqlId}, ExeSqlId: {sqlId}");

                        sqlMap.CacheProvider.Flush();
                        mappedTimes[sqlMap.SqlId] = DateTime.Now;
                    }*/
                }
            }
        }

        private void Flush(RequestContext context)
        {
            Flush($"{context.Scope}.{context.SqlId}");
        }
        #endregion

        #region flushbyinterval
        private void FlushByInterval(SqlMap sqlMap)
        {
            //var sqlId = sqlMap.SqlId;
            var cacheId = sqlMap.Cache.Id;
            var now = DateTime.Now;

            //mappedTimes.AddOrUpdate(sqlId, now, (key, lastFlush) =>
            mappedTimes.AddOrUpdate(cacheId, now, (key, lastFlush) =>
            {
                var lastInterval = now - lastFlush;
                if (lastInterval >= sqlMap.Cache.FlushInterval)
                {
                    logger.LogDebug($"CacheManager FlushCache.OnInterval CacheId: {cacheId}, LastInterval: {lastInterval}");
                    sqlMap.Cache.Provider.Flush();

                    return DateTime.Now;
                }
                return lastFlush;
            });
        }
        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Threading.Tasks;
using System.Text;

using Microsoft.Extensions.Logging;
using Dapper;
using static Dapper.SqlMapper;
using Acesoft.Logger;
using Acesoft.Data.SqlMapper.Caching;
using Acesoft.Config;

namespace Acesoft.Data.SqlMapper
{
    public class SqlMapper : ISqlMapper
    {
        private readonly ILogger logger = LoggerContext.GetLogger<SqlMapper>();
        public ICacheManager CacheManager { get; }
        public IDictionary<string, SqlScope> MappedScopes { get; }

        #region ctor
        public SqlMapper(IList<string> directories)
        {
            MappedScopes = new Dictionary<string, SqlScope>();

            // 先执行scope和sqlmap初始化，再初始化缓存
            if (directories != null)
            {
                Initalize(directories);
            }
            CacheManager = new CacheManager(this);
        }

        private void Initalize(IList<string> directories)
        {
            foreach (var dir in directories)
            {
                var path = App.GetLocalPath(dir);
                if (!Directory.Exists(path))
                {
                    throw new AceException($"Not found the folder: {path}");
                }
                logger.LogDebug($"SqlMapper initalize the folder: {path}");

                var files = Directory.EnumerateFiles(path, "*.config");
                foreach (var file in files)
                {
                    var scope = ConfigContext.GetXmlConfig<SqlScope>(file, (s) =>
                    {
                        logger.LogDebug($"SqlScope has changed with id: {s.Id}");
                        MappedScopes[s.Id] = s;
                        CacheManager.ResetMappedCaches();
                    });
                    MappedScopes.Add(scope.Id, scope);
                }
            }
        }
        #endregion

        #region sqlmap
        public SqlMap GetSqlMap(string sqlScope, string sqlId)
        {
            if (!MappedScopes.TryGetValue(sqlScope, out SqlScope scope))
            {
                throw new AceException($"SqlMapper can't find Scope: {sqlScope}");
            }
            if (!scope.SqlMaps.TryGetValue(sqlId, out SqlMap sqlMap))
            {
                throw new AceException($"SqlMapper can't find SqlMap: {sqlScope}.{sqlId}");
            }
            return sqlMap;
        }

        public SqlMap GetSqlMap(RequestContext ctx)
        {
            if (!MappedScopes.TryGetValue(ctx.Scope, out SqlScope scope))
            {
                throw new AceException($"SqlMapper can't find Scope: {ctx.Scope}");
            }
            if (!scope.SqlMaps.TryGetValue(ctx.SqlId, out SqlMap sqlMap))
            {
                throw new AceException($"SqlMapper can't find SqlMap: {ctx.Scope}.{ctx.SqlId}");
            }
            return sqlMap;
        }
        #endregion

        #region ISqlMapper
        private T DoRequest<T>(ISession s, RequestContext ctx, Func<string, T> func)
        {
            logger.LogInformation($"SqlMapper do request: {ctx.Scope}.{ctx.SqlId}, type: {typeof(T)}");
            
            // 处理参数，附加查询参数，放在前面是因为参数会构造缓存Key
            var map = GetSqlMap(ctx);
            map.AppendSqlParams(ctx);

            // 检查缓存
            var cache = CacheManager[ctx];
            if (cache != null)
            {
                return (T)cache;
            }

            // 获取sql语句
            string sql = map.BuildSql(s, ctx);
            var result = func(sql.Replace(ctx.ExtraParams));
            CacheManager[ctx] = result;
            return result;
        }

        private T DoRequest<T>(RequestContext ctx, Func<T> func)
        {
            logger.LogInformation($"SqlMapper do request: {ctx.Scope}.{ctx.SqlId}, type: {typeof(T)}");

            var cache = CacheManager[ctx];
            if (cache != null)
            {
                return (T)cache;
            }

            var result = func();
            CacheManager[ctx] = result;
            return result;
        }

        private T DoPage<T>(RequestContext ctx, GridRequest grid, Func<SqlMap, PageParam, T> func)
        {
            var map = GetSqlMap(ctx);
            // 附加查询参数，方便生成缓存Key
            map.AppendSqlParams(ctx);

            #region 从sqlmap中获取参数信息
            var param = new PageParam();
            var isTree = map.Params.GetValue("istree", false);

            var table = map.Params.GetValue("table", "");
            param.Table = map.Params.GetValue("operatetable", table);
            param.Columns = map.Params.GetValue("fields", "*");
            param.Where = map.Params.GetValue("where", "");
            param.Groupby = map.Params.GetValue("groupby", "");
            param.Orderby = map.Params.GetValue("orderby", "id");
            param.Page = map.Params.GetValue("page", 1);
            param.PageSize = map.Params.GetValue("pagesize", 20);
            #endregion

            #region 当请求参数存在时，覆盖相应参数
            if (grid != null)
            {
                if (grid.Order.HasValue())
                {
                    var sb = new StringBuilder();
                    var sorts = grid.Sort.Split(',');
                    var orders = grid.Order.Split(',');
                    for (var i = 0; i < sorts.Length; i++)
                    {
                        sb.Append($",{sorts[i]} {orders[i]}");
                    }
                    if (sb.Length > 1)
                    {
                        sb.Remove(0, 1);
                        param.Orderby = sb.ToString();
                    }
                }
                if (grid.Page.HasValue)
                {
                    param.Page = grid.Page.Value;
                }
                if (grid.Rows.HasValue)
                {
                    param.PageSize = grid.Rows.Value;
                }
            }
            #endregion

            #region 处理查询参数
            string q = string.Empty;
            string queryValue = null;
            bool hasFilterParam = false;
            bool hasFilterParamValue = false;
            foreach (var query in map.Query)
            {
                if (query.Value != "qs" && query.Value != "ac")
                {
                    // 拼接条件
                    if (query.Key.StartsWith("_"))
                    {
                        hasFilterParam = true;
                    }
                    queryValue = App.GetQuery(query.Key, "").Trim();
                    if (queryValue.HasValue())
                    {
                        // 此处同步添加ctx的params
                        ctx.Params[query.Key] = queryValue;

                        // 拼接查询条件
                        hasFilterParamValue = hasFilterParam;
                        if (query.Key.StartsWith("in"))
                        {
                            q += $" and ({string.Format(query.Value, queryValue)})";
                        }
                        else
                        {
                            q += $" and ({string.Format(query.Value, queryValue.Split(','))})";
                        }
                    }
                }
            }
            if (q.HasValue() && (!isTree || (isTree && !grid.Id.HasValue)))
            {
                // 当不为树表格 或 树表格顶级数据时，拼接查询条件
                if (param.Where.HasValue())
                {
                    param.Where += q;
                }
                else
                {
                    param.Where = q.Substring(5);
                }
            }
            else if (!isTree || (isTree && !grid.Id.HasValue))
            {
                // 当存在filter参数，且没值的时候取wherenotfilter
                if (hasFilterParam && !hasFilterParamValue)
                {
                    string filter = map.Params.GetValue("wherenotfilter", "");
                    if (filter.HasValue())
                    {
                        if (param.Where.HasValue())
                        {
                            param.Where += " and " + filter;
                        }
                        else
                        {
                            param.Where = filter;
                        }
                    }
                }
            }

            if (isTree)
            {
                string w;
                var asyncWhere = map.Params.GetValue("asyncwhere", "");
                var rootWhere = map.Params.GetValue("rootwhere", "");

                if (grid.Id.HasValue)
                {
                    w = asyncWhere.HasValue() ? asyncWhere : $"parentid={grid.Id.Value}";
                }
                else
                {
                    w = rootWhere.HasValue() ? rootWhere : "1=1";
                }

                if (param.Where.HasValue())
                {
                    param.Where += " and " + w;
                }
                else
                {
                    param.Where = w;
                }
            }
            #endregion

            #region 处理上下文变量
            if (ctx.ExtraParams != null)
            {
                param.Table = param.Table.Replace(ctx.ExtraParams);
                param.Where = param.Where.Replace(ctx.ExtraParams);
                param.Columns = param.Columns.Replace(ctx.ExtraParams);
            }
            param.Where = param.Where.Replace(ctx.Params);
            #endregion

            // 获取数据
            return DoRequest(ctx, () => func(map, param));
        }

        public int Execute(ISession s, RequestContext ctx)
         {
            return DoRequest(s, ctx, (sql) => s.Execute(sql, ctx.DapperParams));
        }

        public Task<int> ExecuteAsync(ISession s, RequestContext ctx)
        {
            return DoRequest(s, ctx, (sql) => s.ExecuteAsync(sql, ctx.DapperParams));
        }

        public object ExecuteScalar(ISession s, RequestContext ctx)
        {
            return DoRequest(s, ctx, (sql) => s.ExecuteScalar(sql, ctx.DapperParams));
        }

        public Task<object> ExecuteScalarAsync(ISession s, RequestContext ctx)
        {
            return DoRequest(s, ctx, (sql) => s.ExecuteScalarAsync(sql, ctx.DapperParams));
        }

        public T ExecuteScalar<T>(ISession s, RequestContext ctx)
        {
            return DoRequest(s, ctx, (sql) => s.ExecuteScalar<T>(sql, ctx.DapperParams));
        }

        public Task<T> ExecuteScalarAsync<T>(ISession s, RequestContext ctx)
        {
            return DoRequest(s, ctx, (sql) => s.ExecuteScalarAsync<T>(sql, ctx.DapperParams));
        }

        public IEnumerable<dynamic> Query(ISession s, RequestContext ctx)
        {
            return DoRequest(s, ctx, (sql) => s.Query(sql, ctx.DapperParams));
        }

        public IEnumerable<T> Query<T>(ISession s, RequestContext ctx)
        {
            return DoRequest(s, ctx, (sql) => s.Query<T>(sql, ctx.DapperParams));
        }

        public IEnumerable<TReturn> Query<TFisrt, TSecond, TReturn>(ISession s, RequestContext ctx, Func<TFisrt, TSecond, TReturn> map)
        {
            return DoRequest(s, ctx, (sql) => s.Query(sql, map, ctx.DapperParams));
        }

        public IEnumerable<TReturn> Query<TFirst, TSecond, TThird, TReturn>(ISession s, RequestContext ctx, Func<TFirst, TSecond, TThird, TReturn> map)
        {
            return DoRequest(s, ctx, (sql) => s.Query(sql, map, ctx.DapperParams));
        }

        public dynamic QueryFirst(ISession s, RequestContext ctx)
        {
            return DoRequest(s, ctx, (sql) => s.QueryFirst(sql, ctx.DapperParams));
        }

        public T QueryFirst<T>(ISession s, RequestContext ctx)
        {
            return DoRequest(s, ctx, (sql) => s.QueryFirst<T>(sql, ctx.DapperParams));
        }

        public dynamic QuerySingle(ISession s, RequestContext ctx)
        {
            return DoRequest(s, ctx, (sql) => s.QuerySingle(sql, ctx.DapperParams));
        }

        public T QuerySingle<T>(ISession s, RequestContext ctx)
        {
            return DoRequest(s, ctx, (sql) => s.QuerySingle<T>(sql, ctx.DapperParams));
        }

        public T QueryMultiple<T>(ISession s, RequestContext ctx, Func<GridReader, T> func)
        {
            return DoRequest(s, ctx, (sql) => 
            {
                using (var reader = s.QueryMultiple(sql, ctx.DapperParams))
                {
                    return func(reader);
                }
            });
        }

        public IDictionary<string, IEnumerable<dynamic>> QueryMultiple(ISession s, RequestContext ctx)
        {
            return DoRequest(s, ctx, (sql) =>
            {
                var map = GetSqlMap(ctx);
                var results = new Dictionary<string, IEnumerable<dynamic>>();
                var datasets = map.Params.GetValue("datasets").Split(',');
                using (var reader = s.QueryMultiple(sql, ctx.DapperParams))
                {
                    foreach (var ds in datasets)
                    {
                        results.Add(ds, reader.Read());
                    }
                }
                return results;
            });
        }

        public DataTable QueryDataTable(ISession s, RequestContext ctx)
        {
            return DoRequest(s, ctx, (sql) => s.QueryDataTable(sql, ctx.DapperParams));
        }

        public DataSet QueryDataSet(ISession s, RequestContext ctx)
        {
            return DoRequest(s, ctx, (sql) => s.QueryDataSet(sql, ctx.DapperParams));
        }

        public GridResponse<dynamic> QueryPage(ISession s, RequestContext ctx, GridRequest request)
        {
            return DoPage(ctx, request, (map, param) =>
            {
                var res = s.QueryPage(param);
                res.Request = request;
                return res;
            });
        }

        public GridResponse<T> QueryPage<T>(ISession s, RequestContext ctx, GridRequest request)
        {
            return DoPage(ctx, request, (map, param) =>
            {
                var res = s.QueryPage<T>(param);
                res.Request = request;
                return res;
            });
        }

        public GridResponse QueryPageTable(ISession s, RequestContext ctx, GridRequest request)
        {
            return DoPage(ctx, request, (map, param) =>
            {
                var res = s.QueryPageTable(param);
                res.Request = request;
                res.Map = map;
                return res;
            });
        }
        #endregion
    }
}

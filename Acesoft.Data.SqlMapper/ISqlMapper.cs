using Acesoft.Data.SqlMapper.Caching;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

using static Dapper.SqlMapper;

namespace Acesoft.Data.SqlMapper
{
    public interface ISqlMapper
    {
        ICacheManager CacheManager { get; }
        IDictionary<string, SqlScope> MappedScopes { get; }

        SqlMap GetSqlMap(string sqlScope, string sqlId);
        SqlMap GetSqlMap(RequestContext ctx);

        int Execute(ISession s, RequestContext ctx);
        Task<int> ExecuteAsync(ISession s, RequestContext ctx);
        object ExecuteScalar(ISession s, RequestContext ctx);
        Task<object> ExecuteScalarAsync(ISession s, RequestContext ctx);
        T ExecuteScalar<T>(ISession s, RequestContext ctx);
        Task<T> ExecuteScalarAsync<T>(ISession s, RequestContext ctx);

        IEnumerable<dynamic> Query(ISession s, RequestContext ctx);
        IEnumerable<T> Query<T>(ISession s, RequestContext ctx);
        IEnumerable<TReturn> Query<TFisrt, TSecond, TReturn>(ISession s, RequestContext ctx, Func<TFisrt, TSecond, TReturn> map);
        IEnumerable<TReturn> Query<TFirst, TSecond, TThird, TReturn>(ISession s, RequestContext ctx, Func<TFirst, TSecond, TThird, TReturn> map);
        dynamic QueryFirst(ISession s, RequestContext ctx);
        T QueryFirst<T>(ISession s, RequestContext ctx);
        dynamic QuerySingle(ISession s, RequestContext ctx);
        T QuerySingle<T>(ISession s, RequestContext ctx);
        IDictionary<string, IEnumerable<dynamic>> QueryMultiple(ISession s, RequestContext ctx);
        T QueryMultiple<T>(ISession s, RequestContext ctx, Func<GridReader, T> func);
        DataTable QueryDataTable(ISession s, RequestContext ctx);
        DataSet QueryDataSet(ISession s, RequestContext ctx);
        GridResponse<dynamic> QueryPage(ISession s, RequestContext ctx, GridRequest request);
        GridResponse<T> QueryPage<T>(ISession s, RequestContext ctx, GridRequest request);
        GridResponse QueryPageTable(ISession s, RequestContext ctx, GridRequest request);
    }
}

using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using static Dapper.SqlMapper;

namespace Acesoft.Data
{
    public interface ISession : IDisposable
    {
        #region ISession
        IStore Store { get; }
        IDbConnection Connection { get; }
        IDbTransaction Transaction { get; }

        bool IsInTransaction { get; }
        void BeginTransaction(IsolationLevel il = IsolationLevel.ReadCommitted);
        void Commit();
        void Rollback();
        #endregion

        #region Execute
        int Execute(string sql, object param = null);
        Task<int> ExecuteAsync(string sql, object param = null);
        object ExecuteScalar(string sql, object param = null);
        Task<object> ExecuteScalarAsync(string sql, object param = null);
        T ExecuteScalar<T>(string sql, object param = null);
        Task<T> ExecuteScalarAsync<T>(string sql, object param = null);
        #endregion

        #region Query
        IEnumerable<dynamic> Query(string sql, object param = null);
        IEnumerable<T> Query<T>(string sql, object param = null);
        IEnumerable<TReturn> Query<TFisrt, TSecond, TReturn>(string sql, Func<TFisrt, TSecond, TReturn> map, object param = null);
        IEnumerable<TReturn> Query<TFirst, TSecond, TThird, TReturn>(string sql, Func<TFirst, TSecond, TThird, TReturn> map, object param = null);
        dynamic QueryFirst(string sql, object param = null);
        T QueryFirst<T>(string sql, object param = null);
        dynamic QuerySingle(string sql, object param = null);
        T QuerySingle<T>(string sql, object param = null);
        IDictionary<string, IEnumerable<dynamic>> QueryMultiple(string sql, string keys, object param = null);
        GridReader QueryMultiple(string sql, object param = null);
        GridResponse<dynamic> QueryPage(PageParam param);
        GridResponse<T> QueryPage<T>(PageParam param);
        GridResponse QueryPageTable(PageParam param);
        #endregion

        #region Get
        T Get<T>(long id) where T : class;
        Task<T> GetAsync<T>(long id) where T : class;
        IEnumerable<T> GetAll<T>() where T : class;
        Task<IEnumerable<T>> GetAllAsync<T>() where T : class;
        #endregion

        #region Table
        DataTable QueryDataTable(string sql, object param = null);

        DataSet QueryDataSet(string sql, object param = null);
        #endregion

        #region Insert
        long Insert<T>(T obj) where T : class;
        Task<int> InsertAsync<T>(T obj) where T : class;
        #endregion

        #region Update
        bool Update<T>(T obj) where T : class;
        Task<bool> UpdateAsync<T>(T obj) where T : class;
        #endregion

        #region Delete
        bool Delete<T>(T obj) where T : class;
        Task<bool> DeleteAsync<T>(T obj) where T : class;
        bool DeleteAll<T>() where T : class;
        Task<bool> DeleteAllAsync<T>() where T : class;
        #endregion
    }
}

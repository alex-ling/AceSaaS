using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

using Dapper;
using Dapper.Contrib.Extensions;
using Microsoft.Extensions.Logging;
using static Dapper.SqlMapper;
using Acesoft.Logger;

namespace Acesoft.Data
{
    public class Session : ISession
    {
        private static ILogger<Session> logger = LoggerContext.GetLogger<Session>();

        #region ISession
        public IStore Store { get; private set; }
        public IDbConnection Connection { get; private set; }
        public IDbTransaction Transaction { get; private set; }
        public bool IsInTransaction => Transaction != null;

        private IsolationLevel isolationLevel;

        public Session(IStore store, IsolationLevel isolationLevel)
        {
            this.Store = store;
            this.isolationLevel = isolationLevel;

            Connection = store.Option.ConnectionFactory.CreateConnection();
            if (Connection.State != ConnectionState.Open)
            {
                Connection.Open();
            }
        }

        public void BeginTranscation()
        {
            BeginTransaction(isolationLevel);
        }

        public void BeginTransaction(IsolationLevel il)
        {
            if (Connection.State == ConnectionState.Closed)
            {
                Connection.Open();
            }
            Transaction = Connection.BeginTransaction(il);

            logger.LogDebug($"Begin {il} transcation with database \"{Store.Option.Name}\"");
        }

        public void Commit()
        {
            logger.LogDebug($"Commit transcation with database \"{Store.Option.Name}\"");

            Transaction.Commit();
            EndTransaction();
        }

        public void Rollback()
        {
            logger.LogDebug($"Rollback transcation with database \"{Store.Option.Name}\"");

            Transaction.Rollback();
            EndTransaction();
        }

        public void Dispose()
        {
            EndTransaction();

            if (Connection != null)
            {
                Store.Option.ConnectionFactory.CloseConnection(Connection);
                Connection = null;
            }

            logger.LogDebug($"Dispose session with database \"{Store.Option.Name}\"");
        }

        private void EndTransaction()
        {
            if (Transaction != null)
            {
                Transaction.Dispose();
                Transaction = null;

                logger.LogDebug($"End transcation with database \"{Store.Option.Name}\"");
            }
        }
        #endregion

        #region Execute
        public int Execute(string sql, object param = null)
        {
            return Connection.Execute(sql, param, Transaction);
        }

        public Task<int> ExecuteAsync(string sql, object param = null)
        {
            return Connection.ExecuteAsync(sql, param, Transaction);
        }

        public object ExecuteScalar(string sql, object param = null)
        {
            return Connection.ExecuteScalar(sql, param, Transaction);
        }

        public Task<object> ExecuteScalarAsync(string sql, object param = null)
        {
            return Connection.ExecuteScalarAsync(sql, param, Transaction);
        }

        public T ExecuteScalar<T>(string sql, object param = null)
        {
            return Connection.ExecuteScalar<T>(sql, param, Transaction);
        }

        public Task<T> ExecuteScalarAsync<T>(string sql, object param = null)
        {
            return Connection.ExecuteScalarAsync<T>(sql, param, Transaction);
        }
        #endregion

        #region Query
        public IEnumerable<dynamic> Query(string sql, object param = null)
        {
            return Connection.Query(sql, param, Transaction);
        }

        public IEnumerable<T> Query<T>(string sql, object param = null)
        {
            return Connection.Query<T>(sql, param, Transaction);
        }

        public IEnumerable<TReturn> Query<TFisrt, TSecond, TReturn>(string sql, Func<TFisrt, TSecond, TReturn> map, object param = null)
        {
            return Connection.Query(sql, map, param, Transaction);
        }

        public IEnumerable<TReturn> Query<TFirst, TSecond, TThird, TReturn>(string sql, Func<TFirst, TSecond, TThird, TReturn> map, object param = null)
        {
            return Connection.Query(sql, map, param, Transaction);
        }

        public dynamic QueryFirst(string sql, object param = null)
        {
            return Connection.QueryFirstOrDefault(sql, param, Transaction);
        }

        public T QueryFirst<T>(string sql, object param = null)
        {
            return Connection.QueryFirstOrDefault<T>(sql, param, Transaction);
        }

        public dynamic QuerySingle(string sql, object param = null)
        {
            return Connection.QuerySingle(sql, param, Transaction);
        }

        public T QuerySingle<T>(string sql, object param = null)
        {
            return Connection.QuerySingle<T>(sql, param, Transaction);
        }

        public IDictionary<string, IEnumerable<dynamic>> QueryMultiple(string sql, string keys, object param = null)
        {
            var results = new Dictionary<string, IEnumerable<dynamic>>();
            var reader = Connection.QueryMultiple(sql, param, Transaction);
            foreach (var key in keys.Split(','))
            {
                results.Add(key, reader.Read());
            }
            return results;
        }

        public GridReader QueryMultiple(string sql, object param = null)
        {
            return Connection.QueryMultiple(sql, param, Transaction);
        }

        public GridResponse<dynamic> QueryPage(PageParam param)
        {
            var @params = new DynamicParameters();
            @params.Add("table", param.Table);
            @params.Add("fields", param.Columns);
            @params.Add("where", param.Where);
            @params.Add("groupby", param.Groupby);
            @params.Add("orderby", param.Orderby);
            @params.Add("pagesize", param.PageSize);
            @params.Add("pageindex", param.Page);
            @params.Add("pagecount", 0, DbType.Int32, ParameterDirection.Output);
            @params.Add("count", 0, DbType.Int32, ParameterDirection.Output);
            var datas = Connection.Query("ace_paging", @params, Transaction, commandType: CommandType.StoredProcedure);
            return new GridResponse<dynamic>
            {
                Total = @params.Get<int>("@count"),
                PageCount = @params.Get<int>("@pagecount"),
                Data = datas
            };
        }

        public GridResponse<T> QueryPage<T>(PageParam param)
        {
            var @params = new DynamicParameters();
            @params.Add("table", param.Table);
            @params.Add("fields", param.Columns);
            @params.Add("where", param.Where);
            @params.Add("groupby", param.Groupby);
            @params.Add("orderby", param.Orderby);
            @params.Add("pagesize", param.PageSize);
            @params.Add("pageindex", param.Page);
            @params.Add("pagecount", 0, DbType.Int32, ParameterDirection.Output);
            @params.Add("count", 0, DbType.Int32, ParameterDirection.Output);
            var datas = Connection.Query<T>("ace_paging", @params, Transaction, commandType: CommandType.StoredProcedure);
            return new GridResponse<T>
            {
                Total = @params.Get<int>("@count"),
                PageCount = @params.Get<int>("@pagecount"),
                Data = datas
            };
        }

        public GridResponse QueryPageTable(PageParam param)
        {
            var @params = new DynamicParameters();
            @params.Add("table", param.Table);
            @params.Add("fields", param.Columns);
            @params.Add("where", param.Where);
            @params.Add("groupby", param.Groupby);
            @params.Add("orderby", param.Orderby);
            @params.Add("pagesize", param.PageSize);
            @params.Add("pageindex", param.Page);
            @params.Add("pagecount", 0, DbType.Int32, ParameterDirection.Output);
            @params.Add("count", 0, DbType.Int32, ParameterDirection.Output);

            using (var dr = Connection.ExecuteReader("ace_paging", @params, Transaction, commandType: CommandType.StoredProcedure))
            {
                return new GridResponse
                {
                    Data = dr.ToDataTable(),
                    Total = @params.Get<int>("@count"),
                    PageCount = @params.Get<int>("@pagecount")
                };
            }
        }
        #endregion

        #region Get
        public T Get<T>(long id) where T : class
        {
            return Connection.Get<T>(id, Transaction);
        }

        public Task<T> GetAsync<T>(long id) where T : class
        {
            return Connection.GetAsync<T>(id, Transaction);
        }

        public IEnumerable<T> GetAll<T>() where T : class
        {
            return Connection.GetAll<T>(Transaction);
        }

        public Task<IEnumerable<T>> GetAllAsync<T>() where T : class
        {
            return Connection.GetAllAsync<T>(Transaction);
        }
        #endregion

        #region Table
        public DataTable QueryDataTable(string sql, object param = null)
        {
            using (var dr = Connection.ExecuteReader(sql, param, Transaction))
            {
                return dr.ToDataTable();
            }
        }

        public DataSet QueryDataSet(string sql, object param = null)
        {
            using (var dr = Connection.ExecuteReader(sql, param, Transaction))
            {
                return dr.ToDataSet();
            }
        }
        #endregion

        #region Insert
        public long Insert<T>(T obj) where T : class
        {
            return Connection.Insert(obj, Transaction);
        }

        public Task<int> InsertAsync<T>(T obj) where T : class
        {
            return Connection.InsertAsync(obj, Transaction);
        }
        #endregion

        #region Update
        public bool Update<T>(T obj) where T : class
        {
            return Connection.Update<T>(obj, Transaction);
        }

        public Task<bool> UpdateAsync<T>(T obj) where T : class
        {
            return Connection.UpdateAsync<T>(obj, Transaction);
        }
        #endregion

        #region Delete
        public bool Delete<T>(T obj) where T : class
        {
            return Connection.Delete<T>(obj, Transaction);
        }

        public Task<bool> DeleteAsync<T>(T obj) where T : class
        {
            return Connection.DeleteAsync<T>(obj, Transaction);
        }

        public bool DeleteAll<T>() where T : class
        {
            return Connection.DeleteAll<T>(Transaction);
        }

        public Task<bool> DeleteAllAsync<T>() where T : class
        {
            return Connection.DeleteAllAsync<T>(Transaction);
        }
        #endregion
    }
}
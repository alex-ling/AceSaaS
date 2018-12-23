using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using Dapper;
using Dapper.Contrib.Extensions;

namespace Acesoft.Data
{
    public class Repository<T> : IRespository<T> where T : EntityBase
    {
        public ISession Session => DataContext.Session;

        #region Get
        public T Get(long id)
        {
            return Session.Get<T>(id);
        }

        public Task<T> GetAsync(long id)
        {
            return Session.GetAsync<T>(id);
        }

        public IEnumerable<T> GetAll()
        {
            return Session.GetAll<T>();
        }

        public Task<IEnumerable<T>> GetAllAsync()
        {
            return Session.GetAllAsync<T>();
        }
        #endregion

        #region Gets
        public IEnumerable<T> GetList(string sql, object param)
        {
            return Session.Query<T>(sql, param);
        }
        #endregion

        #region Insert
        public long Insert(T obj)
        {
            return Session.Insert(obj);
        }

        public Task<int> InsertAsync(T obj)
        {
            return Session.InsertAsync(obj);
        }

        public long Insert(IEnumerable<T> objs)
        {
            return Session.Insert(objs);
        }

        public Task<int> InsertAsync(IEnumerable<T> objs)
        {
            return Session.InsertAsync(objs);
        }
        #endregion

        #region Update
        public bool Update(T obj)
        {
            return Session.Update<T>(obj);
        }

        public Task<bool> UpdateAsync(T obj)
        {
            return Session.UpdateAsync<T>(obj);
        }

        public bool Update(IEnumerable<T> objs)
        {
            return Session.Update<IEnumerable<T>>(objs);
        }

        public Task<bool> UpdateAsync(IEnumerable<T> objs)
        {
            return Session.UpdateAsync<IEnumerable<T>>(objs);
        }
        #endregion

        #region Delete
        public bool Delete(T obj)
        {
            return Session.Delete<T>(obj);
        }

        public Task<bool> DeleteAsync(T obj)
        {
            return Session.DeleteAsync<T>(obj);
        }

        public bool Delete(IEnumerable<T> objs)
        {
            return Session.Delete(objs);
        }

        public Task<bool> DeleteAsync(IEnumerable<T> objs)
        {
            return Session.DeleteAsync(objs);
        }

        public bool DeleteAll()
        {
            return Session.DeleteAll<T>();
        }

        public Task<bool> DeleteAllAsync()
        {
            return Session.DeleteAllAsync<T>();
        }
        #endregion
    }
}

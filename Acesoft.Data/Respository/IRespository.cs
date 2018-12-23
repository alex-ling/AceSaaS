using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Acesoft.Data
{
    public interface IRespository<T> where T : EntityBase
    {
        ISession Session { get; }

        T Get(long id);
        Task<T> GetAsync(long id);
        IEnumerable<T> GetAll();
        Task<IEnumerable<T>> GetAllAsync();

        IEnumerable<T> GetList(string query, object param);

        long Insert(T obj);
        Task<int> InsertAsync(T obj);
        long Insert(IEnumerable<T> objs);
        Task<int> InsertAsync(IEnumerable<T> objs);

        bool Update(T obj);
        Task<bool> UpdateAsync(T obj);
        bool Update(IEnumerable<T> objs);
        Task<bool> UpdateAsync(IEnumerable<T> objs);

        bool Delete(T obj);
        Task<bool> DeleteAsync(T obj);
        bool Delete(IEnumerable<T> objs);
        Task<bool> DeleteAsync(IEnumerable<T> objs);
        bool DeleteAll();
        Task<bool> DeleteAllAsync();
    }
}

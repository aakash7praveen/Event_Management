using System.Collections.Generic;
using System.Threading.Tasks;

namespace EventManagementAPI.Repositories.Generics
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T?> GetByIdAsync(string sql, object param);
        Task<IEnumerable<T>> GetAllAsync(string sql);
        Task<IEnumerable<T>> FindAsync(string sql, object? param = null);
        Task<int> AddAsync(string sql, T entity);
        Task<int> UpdateAsync(string sql, T entity);
        Task<int> DeleteAsync(string sql, object param);
    }
}

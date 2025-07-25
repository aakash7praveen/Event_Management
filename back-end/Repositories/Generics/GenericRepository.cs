using Dapper;
using EventManagementAPI.Helpers;
using EventManagementAPI.Repositories.Generics;
using EventManagementAPI.Repositories.Interfaces;
using System.Data;

namespace EventManagementAPI.Repositories.Generics
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly IDbConnection _db;

        public GenericRepository(DbHelper dbHelper)
        {
            _db = dbHelper.CreateConnection();
        }

        public async Task<T?> GetByIdAsync(string sql, object param)
        {
            return await _db.QueryFirstOrDefaultAsync<T>(sql, param);
        }

        public async Task<IEnumerable<T>> GetAllAsync(string sql)
        {
            return await _db.QueryAsync<T>(sql);
        }

        public async Task<IEnumerable<T>> FindAsync(string sql, object? param = null)
        {
            return await _db.QueryAsync<T>(sql, param);
        }

        public async Task<int> AddAsync(string sql, T entity)
        {
            return await _db.ExecuteAsync(sql, entity);
        }

        public async Task<int> UpdateAsync(string sql, T entity)
        {
            return await _db.ExecuteAsync(sql, entity);
        }

        public async Task<int> DeleteAsync(string sql, object param)
        {
            return await _db.ExecuteAsync(sql, param);
        }
    }
}

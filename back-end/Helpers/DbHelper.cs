using System.Data;
using System.Data.SqlClient;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace EventManagementAPI.Helpers
{
    public class DbHelper
    {
        private readonly string _connectionString;

        public DbHelper(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        private IDbConnection Connection => new SqlConnection(_connectionString);

        public async Task<IEnumerable<T>> QueryAsync<T>(string sql, object? parameters = null)
        {
            using var conn = Connection;
            return await conn.QueryAsync<T>(sql, parameters);
        }

        public async Task<T?> QuerySingleAsync<T>(string sql, object? parameters = null)
        {
            using var conn = Connection;
            return await conn.QuerySingleOrDefaultAsync<T>(sql, parameters);
        }

        public async Task<int> ExecuteAsync(string sql, object? parameters = null)
        {
            using var conn = Connection;
            return await conn.ExecuteAsync(sql, parameters);
        }
    }
}

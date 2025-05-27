using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Data.SqlClient;
using WorkSoftCase.Repository.Interfaces.IRepository;

namespace WorkSoftCase.Repository.Impl
{
    public class DapperRepository<T> : IRepository<T> where T : class
    {
        protected readonly IDbConnection _connection;

        public DapperRepository(IDbConnection connection)
        {
            _connection = connection;
        }
        private string GetTableName()
        {
            var tableAttr = typeof(T).GetCustomAttribute<TableAttribute>();
            if (tableAttr == null)
                throw new InvalidOperationException("Table attribute not found.");
            var tableName = tableAttr.Name;
            return tableName;
        }
        public async Task<bool> AddAsync(T entity)
        {
            var type = typeof(T);
            var properties = type.GetProperties();
            var tableName = GetTableName();

            var columnNames = string.Join(", ", properties.Select(p => p.Name));
            var paramNames = string.Join(", ", properties.Select(p => "@" + p.Name));

            var sql = $"INSERT INTO {tableName} ({columnNames}) VALUES ({paramNames})";

            var result = await _connection.ExecuteAsync(sql, entity);
            return result > 0;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var tableName = GetTableName();

            var sql = $"DELETE FROM {tableName} WHERE Id = @Id";
            var result = await _connection.ExecuteAsync(sql, new { Id = id });
            return result > 0;
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            var tableName = GetTableName();

            var sql = $"SELECT * FROM {tableName}";
            var result = await _connection.QueryAsync<T>(sql);
            return result;
        }

        public async Task<T?> GetByIdAsync(Guid id)
        {
            var tableName = GetTableName();

            var sql = $"SELECT * FROM {tableName} WHERE Id = @Id";
            var result = await _connection.QuerySingleOrDefaultAsync<T>(sql, new { Id = id });
            return result;
        }

        public async Task<bool> UpdateAsync(T entity)
        {
            var type = typeof(T);
            var properties = type.GetProperties().Where(p => p.Name != "Id").ToArray();

            var tableName = GetTableName();
            var setField = string.Join(", ", properties.Select(p => $"{p.Name} = @{p.Name}"));
            var sql = new StringBuilder();
            sql.Append($"UPDATE {tableName} SET {setField} WHERE Id = @Id");

            var result = await _connection.ExecuteAsync(sql.ToString(), entity);
            return result > 0;
        }

        public async Task<bool> AddRangeAsync(IEnumerable<T> list)
        {
            var type = typeof(T);
            var properties = type.GetProperties().ToArray();
            var tableName = GetTableName();
            var columnNames = string.Join(", ", properties.Select(p => p.Name));
            var paramNames = string.Join(", ", properties.Select(p => "@" + p.Name));

            var sql = $"INSERT INTO {tableName} ({columnNames}) VALUES ({paramNames})";
            //if any problem occured when transaction proccess, rollback all changes
            if (_connection.State != ConnectionState.Open)
            {
                if (_connection is SqlConnection sqlConnection)
                    await sqlConnection.OpenAsync();
                else
                    _connection.Open();
            }

            using (var transaction = _connection.BeginTransaction())
            {
                try
                {
                    foreach (var entity in list)
                    {
                        var result = await _connection.ExecuteAsync(sql, entity, transaction);
                        if (result <= 0)
                        {
                            transaction.Rollback();
                            return false;
                        }
                    }

                    transaction.Commit();
                    return true;
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
                finally
                {
                    _connection.Close();
                }
            }
        }
    }
}
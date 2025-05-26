using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Data.SqlClient;

namespace WorkSoftCase.Database
{
    public interface IDatabaseConnection
    {
        Task<IDbConnection> ConnectAsync(CancellationToken token = default);
    }

    public class DbConnection : IDatabaseConnection
    {
        private readonly string _connectionString;

        public DbConnection(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<IDbConnection> ConnectAsync(CancellationToken token = default)
        {
            var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync(token);
            return connection;
        }
    }
}

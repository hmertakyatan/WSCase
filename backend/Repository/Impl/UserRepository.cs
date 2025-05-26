using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using WorkSoftCase.Entities;
using WorkSoftCase.Repository.Interfaces;

namespace WorkSoftCase.Repository.Impl
{
    public class UserRepository : DapperRepository<User>, IUserRepository
    {


        public UserRepository(IDbConnection connection) : base(connection)
        {

        }

        public async Task<User?> GetUserByUsername(string username)
        {
            var sql = "SELECT * FROM Users WHERE UserName = @Username";
            var user = await _connection.QuerySingleOrDefaultAsync<User>(sql, new { Username = username });
            return user;
        }
        public async Task<bool> IsUserNameExistsAsync(string username)
        {
            var sql = "SELECT COUNT(1) FROM Users WHERE UserName = @UserName";
            var count = await _connection.ExecuteScalarAsync<int>(sql, new { UserName = username });
            return count > 0;
        }
        
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkSoftCase.Entities;
using WorkSoftCase.Repository.Impl;
using WorkSoftCase.Repository.Interfaces.IRepository;

namespace WorkSoftCase.Repository.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User?> GetUserByUsername(string username);
        Task<bool> IsUserNameExistsAsync(string username);
    }

    
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkSoftCase.Dtos.Requests;
using WorkSoftCase.Dtos.Responses;
using WorkSoftCase.Entities;

namespace WorkSoftCase.Services.Interfaces
{
    public interface IAuthService
    {
        Task<bool> RegisterAsync(UserRequest request);
        Task<string> LoginAsync(LoginRequest request);
        public string HashPassword(string password);
        public bool VerifyPassword(string hashedPassword, string providedPassword);
        public string GenerateToken(string username);
    }
}
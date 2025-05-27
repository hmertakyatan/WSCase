using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkSoftCase.Dtos.Requests;
using WorkSoftCase.Dtos.Responses;
using WorkSoftCase.Entities;
using WorkSoftCase.Services.Results;

namespace WorkSoftCase.Services.Interfaces
{
    public interface IAuthService
    {
        Task<Result<object>> RegisterAsync(UserRequest request);
        Task<Result<string>> LoginAsync(LoginRequest request);
        public string HashPassword(string password);
        public bool VerifyPassword(string hashedPassword, string providedPassword);
        public string GenerateToken(string username);
    }
}
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using WorkSoftCase.Dtos.Requests;
using WorkSoftCase.Dtos.Responses;
using WorkSoftCase.Entities;
using WorkSoftCase.Exceptions;
using WorkSoftCase.Repository.Interfaces;
using WorkSoftCase.Repository.Interfaces.IRepository;
using WorkSoftCase.Services.Interfaces;

namespace WorkSoftCase.Services.Impl
{
    public class AuthService : IAuthService
    {
        private readonly PasswordHasher<string> _hasher = new();
        private readonly IConfiguration _config;
        private readonly IUserRepository _userRepository;

        public AuthService(IConfiguration config, IUserRepository userRepository)
        {
            _config = config;
            _userRepository = userRepository;
        }
        //TODO: REFRESH TOKEN
        public string GenerateToken(string username)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(int.Parse(_config["Jwt:ExpireMinutes"]!)),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string HashPassword(string password)
        {
            return _hasher.HashPassword(null, password);
        }

        public async Task<string> LoginAsync(LoginRequest loginRequest)
        {
            var user = await _userRepository.GetUserByUsername(loginRequest.Username);
            if (user == null || !VerifyPassword(user.UserPassword, loginRequest.Password))
                throw new UnauthorizedAccessException("Kullanıcı adı veya şifre hatalı.");  

            var token = GenerateToken(user.UserName);
            return token;
        }

        public async Task<bool> RegisterAsync(UserRequest request)
        {
            var exist = await _userRepository.IsUserNameExistsAsync(request.UserName);
            if (exist)
                throw new ConflictException("Bu kullanıcı adı zaten mevcut.");

            var hashedPassword = HashPassword(request.Password);
            var user = new User
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                UserName = request.UserName,
                CreateDate = DateTime.UtcNow,
                UserPassword = hashedPassword
            };

            var success = await _userRepository.AddAsync(user);
            if (!success)
                throw new DatabaseOperationException("Kullanıcı eklenemedi.");

            return true;
        }
        

        public bool VerifyPassword(string hashedPassword, string providedPassword)
        {
            var result = _hasher.VerifyHashedPassword(null, hashedPassword, providedPassword);
            return result == PasswordVerificationResult.Success;
        }
    }
}
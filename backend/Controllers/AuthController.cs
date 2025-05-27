using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WorkSoftCase.Dtos.Requests;
using WorkSoftCase.Dtos.Responses;
using WorkSoftCase.Entities;
using WorkSoftCase.Services.Interfaces;

namespace WorkSoftCase.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ApiResponse<object>.ErrorMessage(HttpContext, "Geçersiz kullanıcı verisi.", 400));

            try
            {
                var result = await _authService.RegisterAsync(request);
                var response = ApiResponse<object>.FromResult(HttpContext, result);
                return StatusCode(response.StatusCode, response);
            }
            catch (Exception)
            {
                return StatusCode(500, ApiResponse<object>.ErrorMessage(HttpContext, "Beklenmeyen bir hata oluştu.", 500));
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ApiResponse<object>.ErrorMessage(HttpContext, "Geçersiz giriş verisi.", 400));

            try
            {
                var result = await _authService.LoginAsync(request);
                var response = ApiResponse<string>.FromResult(HttpContext, result);
                return StatusCode(response.StatusCode, response);
            }
            catch (Exception)
            {
                return StatusCode(500, ApiResponse<object>.ErrorMessage(HttpContext, "Beklenmeyen bir hata oluştu.", 500));
            }
        }
    }
}

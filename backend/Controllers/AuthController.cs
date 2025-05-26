using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WorkSoftCase.Dtos.Requests;
using WorkSoftCase.Dtos.Responses;
using WorkSoftCase.Entities;
using WorkSoftCase.Exceptions;
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
            try
            {
                var success = await _authService.RegisterAsync(request);

                if (!success)
                    return BadRequest(ApiResponse<object>.ErrorMessage("Kayıt işlemi başarısız oldu."));
                
                return Ok(ApiResponse<object>.SuccessMessage("Kayıt başarılı."));
            }
            catch (ConflictException ex)
            {
                return Conflict(ApiResponse<string>.ErrorMessage(ex.Message, statusCode: 409));
            }
            catch (DatabaseOperationException ex)
            {
                return StatusCode(500, ApiResponse<string>.ErrorMessage(ex.Message, statusCode:500));
            }
            catch (Exception)
            {
                return StatusCode(500, ApiResponse<string>.ErrorMessage("Beklenmeyen bir hata oluştu."));
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            try
            {
                var token = await _authService.LoginAsync(request);
                if (string.IsNullOrEmpty(token))
                    return StatusCode(500, ApiResponse<string>.ErrorMessage("Token oluşturulamadı."));
                    
                return Ok(ApiResponse<string>.SuccessMessage("Giriş başarılı.", data: token));
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ApiResponse<object>.ErrorMessage(ex.Message, statusCode: 401));
            }
            catch (Exception)
            {
                return StatusCode(500, ApiResponse<object>.ErrorMessage("Beklenmeyen bir hata oluştu."));
            }
        }
    }
}
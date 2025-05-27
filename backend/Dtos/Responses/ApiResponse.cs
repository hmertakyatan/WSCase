using System;
using WorkSoftCase.Services.Results;

namespace WorkSoftCase.Dtos.Responses
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public T? Data { get; set; }
        public int StatusCode { get; set; }

        public string? RequestId { get; set; }
        public string? TraceId { get; set; }
        public string? HttpMethod { get; set; }
        public string? Path { get; set; }
        public DateTime Timestamp { get; set; }

        public static ApiResponse<T> FromResult(HttpContext context, Result<T> result)
        {
            return new ApiResponse<T>
            {
                Success = result.IsSuccess,
                Message = result.Message,
                Data = result.Data,
                StatusCode = result.StatusCode,
                RequestId = context.TraceIdentifier,
                TraceId = context.TraceIdentifier,
                HttpMethod = context.Request.Method,
                Path = context.Request.Path,
                Timestamp = DateTime.UtcNow
            };
        }
        public static ApiResponse<T> ErrorMessage(HttpContext context, string message, int statusCode)
        {
            return new ApiResponse<T>
            {
                Success = false,
                Message = message,
                Data = default,
                StatusCode = statusCode,
                RequestId = context.TraceIdentifier,
                TraceId = context.TraceIdentifier,
                HttpMethod = context.Request.Method,
                Path = context.Request.Path,
                Timestamp = DateTime.UtcNow
            };
        }

        public static ApiResponse<T> SuccessMessage(HttpContext context, string message, T? data = default, int statusCode = 200)
        {
            return new ApiResponse<T>
            {
                Success = true,
                Message = message,
                Data = data,
                StatusCode = statusCode,
                RequestId = context.TraceIdentifier,
                TraceId = context.TraceIdentifier,
                HttpMethod = context.Request.Method,
                Path = context.Request.Path,
                Timestamp = DateTime.UtcNow
            };
        }
    }
        
}


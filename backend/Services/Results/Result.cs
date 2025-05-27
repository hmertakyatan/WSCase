using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WorkSoftCase.Services.Results
{
    public class Result<T>
{
    public bool IsSuccess { get; set; }
    public string? Message { get; set; }
    public T? Data { get; set; }
    public int StatusCode { get; set; }

    public static Result<T> Success(T? data = default, string? message = null, int statusCode = 200)
        {
            return new Result<T>
            {
                IsSuccess = true,
                Message = message,
                Data = data,
                StatusCode = statusCode
            };
        }

    public static Result<T> Failure(string message, int statusCode = 500)
    {
        return new Result<T>
        {
            IsSuccess = false,
            Message = message,
            Data = default,
            StatusCode = statusCode
        };
    }
}
}
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using EmployeeManagement.Domain.Exceptions;
using ValidationException = EmployeeManagement.Domain.Exceptions.ValidationException;

namespace EmployeeManagement.API.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly IHostEnvironment _env;

        public ExceptionMiddleware(
            RequestDelegate next,
            ILogger<ExceptionMiddleware> logger,
            IHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing the request");
                await HandleExceptionAsync(context, ex);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            
            var response = exception switch
            {
                ValidationException validationEx => new ErrorResponse 
                { 
                    Message = "Validation error",
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    Errors = validationEx.Errors.Select(e => e.ErrorMessage).ToList()
                },
                NotFoundException notFoundEx => new ErrorResponse 
                { 
                    Message = notFoundEx.Message,
                    StatusCode = (int)HttpStatusCode.NotFound
                },
                BusinessException businessEx => new ErrorResponse 
                { 
                    Message = businessEx.Message,
                    StatusCode = (int)HttpStatusCode.UnprocessableEntity
                },
                _ => new ErrorResponse 
                { 
                    Message = exception.Message ?? "An unexpected error occurred",
                    StatusCode = (int)HttpStatusCode.InternalServerError
                }
            };

            context.Response.StatusCode = response.StatusCode;
            await context.Response.WriteAsync(JsonSerializer.Serialize(response, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            }));
        }
    }

    public class ErrorResponse
    {
        public string Message { get; set; }
        public int StatusCode { get; set; }
        public List<string> Errors { get; set; } = new List<string>();
    }
}
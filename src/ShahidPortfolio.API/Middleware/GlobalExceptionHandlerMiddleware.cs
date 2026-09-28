using System.Net;
using System.Text.Json;
using ShahidPortfolio.Application.Common.Exceptions;
using ShahidPortfolio.Application.Common.Models;

namespace ShahidPortfolio.API.Middleware;

public class GlobalExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionHandlerMiddleware> _logger;

    public GlobalExceptionHandlerMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandlerMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        var response = exception switch
        {
            ValidationException valEx => new
            {
                StatusCode = (int)HttpStatusCode.BadRequest,
                Body = ApiResponse<object>.Fail("Validation failed.", valEx.Errors, (int)HttpStatusCode.BadRequest)
            },
            NotFoundException notFoundEx => new
            {
                StatusCode = (int)HttpStatusCode.NotFound,
                Body = ApiResponse<object>.Fail(notFoundEx.Message, null, (int)HttpStatusCode.NotFound)
            },
            UnauthorizedAccessException => new
            {
                StatusCode = (int)HttpStatusCode.Unauthorized,
                Body = ApiResponse<object>.Fail("Unauthorized access.", null, (int)HttpStatusCode.Unauthorized)
            },
            _ => new
            {
                StatusCode = (int)HttpStatusCode.InternalServerError,
                Body = ApiResponse<object>.Fail("An unexpected error occurred. Please try again later.", null, (int)HttpStatusCode.InternalServerError)
            }
        };

        if (response.StatusCode == (int)HttpStatusCode.InternalServerError)
        {
            _logger.LogError(exception, "Unhandled exception: {Message}", exception.Message);
        }

        context.Response.StatusCode = response.StatusCode;

        var jsonOptions = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
        await context.Response.WriteAsync(JsonSerializer.Serialize(response.Body, jsonOptions));
    }
}

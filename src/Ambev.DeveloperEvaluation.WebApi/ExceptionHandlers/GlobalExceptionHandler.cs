using System.Net.Mime;
using System.Text.Json;
using Ambev.DeveloperEvaluation.WebApi.Common;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.WebUtilities;

namespace Ambev.DeveloperEvaluation.WebApi.ExceptionHandlers;

/// <summary>
/// Handle exceptions by providing informative details about the faced problem, avoiding exposing
/// sensitive information from exceptions to external users.
/// To learn more about, please read:
/// <see href="https://learn.microsoft.com/en-us/aspnet/core/fundamentals/middleware/?view=aspnetcore-8.0#middleware-order">Microsoft documentation about middleware ordering.</see>
/// </summary>
public sealed class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    private static readonly JsonSerializerOptions DefaultJsonSerializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        httpContext.Response.ContentType = MediaTypeNames.Application.Json;
        httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;

        ApiResponse unhandledExceptionResponse = new()
        {
            Success = false,
            Message = ReasonPhrases.GetReasonPhrase(StatusCodes.Status500InternalServerError),
        };

        _logger.LogError("An unhandled exception was caught with message: {ExceptionMessage}", exception.Message);
        await httpContext.Response.WriteAsJsonAsync(unhandledExceptionResponse, DefaultJsonSerializerOptions, cancellationToken);
        return httpContext.Response.HasStarted;
    }   
}
using System.Net.Mime;
using System.Text.Json;
using Ambev.DeveloperEvaluation.Common.Validation;
using Ambev.DeveloperEvaluation.WebApi.Common;
using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;

namespace Ambev.DeveloperEvaluation.WebApi.ExceptionHandlers;

public sealed class ValidationExceptionHandler : IExceptionHandler
{
    private readonly ILogger<ValidationExceptionHandler> _logger;
    private static readonly JsonSerializerOptions DefaultJsonSerializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    public ValidationExceptionHandler(ILogger<ValidationExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        // We only handle ValidationException types in this handler
        if (exception is not ValidationException validationException) return false;
        
        _logger.LogInformation("Handling Validation Exception with message: {ValidationExceptionMessage}", validationException.Message);
        await HandleValidationExceptionAsync(httpContext, validationException, cancellationToken);
        return httpContext.Response.HasStarted;
    }

    private static Task HandleValidationExceptionAsync(HttpContext context, ValidationException validationException, CancellationToken cancellationToken)
    {
        context.Response.ContentType = MediaTypeNames.Application.ProblemJson;
        context.Response.StatusCode = StatusCodes.Status400BadRequest;

        ApiResponse validationFailedResponse = new(validationException.Errors);

        return context.Response.WriteAsJsonAsync(validationFailedResponse, DefaultJsonSerializerOptions, cancellationToken);
    }
}
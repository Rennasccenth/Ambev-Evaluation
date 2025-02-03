using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Common.ExceptionHandlers;

/// <summary>
/// Handle exceptions by providing informative details about the faced problem, avoiding exposing
/// sensitive information from exceptions to external users.
/// To learn more about, please read:
/// <see href="https://learn.microsoft.com/en-us/aspnet/core/fundamentals/middleware/?view=aspnetcore-8.0#middleware-order">Microsoft documentation about middleware ordering.</see>
/// <seealso href="https://datatracker.ietf.org/doc/html/rfc7807">Latest Problem Details RFC.</seealso>
/// </summary>
public sealed class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;
    private readonly IProblemDetailsService _problemDetailsService;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger,
        IProblemDetailsService problemDetailsService)
    {
        _logger = logger;
        _problemDetailsService = problemDetailsService;
    }

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        _logger.LogError(exception, "An error occurred while processing the request.");

        ProblemDetails problemDetails = new()
        {
            Status = ExceptionStatusCodeMapper.GetStatusCode(exception),
            Type = exception.GetType().Name,
            Title = ExceptionStatusCodeMapper.GetTitle(exception),
            Detail = exception.Message,
        };

        var traceId = httpContext.Features.Get<IHttpActivityFeature>()?.Activity.TraceId;
        if (traceId is not null)
        {
            problemDetails.Extensions.TryAdd("traceId", traceId.ToString());
        }

        if (exception is ValidationException validationException)
        {
            problemDetails.Extensions["errors"] = validationException.Errors
                .GroupBy(e => e.PropertyName)
                .ToDictionary(
                    group => group.Key,
                    failures => failures.Select(e => e.ErrorMessage).ToArray()
                );
        }

        return await _problemDetailsService.TryWriteAsync(new ProblemDetailsContext
        {
            HttpContext = httpContext,
            ProblemDetails = problemDetails
        });
    }   
}
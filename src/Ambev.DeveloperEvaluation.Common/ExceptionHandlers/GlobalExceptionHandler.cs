using System.Diagnostics;
using System.Text.Json;
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
        _logger.LogCritical("An unhandled exception was caught with message: {Exception}", JsonSerializer.Serialize(exception.Message));
        if (exception.InnerException is not null)
            _logger.LogCritical("Inner exception was caught with message: {Exception}", JsonSerializer.Serialize(exception.InnerException.Message));

        return await _problemDetailsService.TryWriteAsync(new ProblemDetailsContext
        {
            HttpContext = httpContext,
            Exception = exception,
            ProblemDetails = new ProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Type = exception.GetType().Name,
                Title = "An unexpected error has occurred.",
                Detail = exception.Message,
                Extensions =
                {
                    { "traceId", httpContext.Features.Get<IHttpActivityFeature>()?.Activity.TraceId  }
                }
            }
        });
    }   
}
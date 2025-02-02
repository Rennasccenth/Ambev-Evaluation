using System.Reflection;
using System.Text.Json;
using Ambev.DeveloperEvaluation.Common.Errors;
using Ambev.DeveloperEvaluation.Common.Results;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Common.Validation;

/// <summary>
/// Ensure that every <see cref="IRequest"/> that returns a <see cref="TResponse"/> of a <see cref="ApplicationResult{T}"/>
/// will be validated before reaching the <see cref="IRequestHandler{TRequest,TResponse}"/>.
/// 
/// </summary>
/// <typeparam name="TRequest"><see cref="IRequest"/></typeparam>
/// <typeparam name="TResponse"><see cref="ApplicationResult{T}"/> where T is the success result.</typeparam>
public class ApplicationValidationBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : IApplicationResult
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;
    private readonly ILogger<ApplicationValidationBehavior<TRequest, TResponse>> _logger;

    public ApplicationValidationBehavior(
        IEnumerable<IValidator<TRequest>> validators,
        ILogger<ApplicationValidationBehavior<TRequest, TResponse>> logger)
    {
        _validators = validators;
        _logger = logger;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Validating request {request}", request);

        if (!_validators.Any())
        {
            _logger.LogError("Skipping default validation pipeline. No validators registered for request {requestType}",
                typeof(TRequest).Name);
            return await next();
        }

        ValidationContext<TRequest> context = new(request);
        ValidationResult[] validationResults = await Task
            .WhenAll(_validators.Select(v => v.ValidateAsync(context, cancellationToken)));

        if (validationResults.All(result => result.IsValid)) return await next();

        _logger.LogTrace("Validation errors occurred in request {request}", JsonSerializer.Serialize(request));
        if (IsGenericApplicationResult(typeof(TResponse), out Type? successResultType))
        {
            ApplicationError error = ApplicationError.ValidationError(validationResults);
            var failureResult = CreateFailureResult(successResultType!, error);
            return (TResponse)failureResult;
        }

        _logger.LogCritical("{TResponse} is not a ApplicationResult<T>", typeof(TResponse).Name);
        throw new InvalidOperationException($"Validation of {typeof(TRequest).Name} failed, but {typeof(TResponse).Name} is not a CommandResult<T>. Cannot proper return failure result.");
    }

    /// <summary>
    /// Helper method to check if <see cref="TResponse"/> is a <see cref="ApplicationResult{T}"/>
    /// </summary>
    /// <param name="type">Verifying <see cref="Type"/></param>
    /// <param name="successResultType">The success result type T in <see cref="ApplicationResult{T}"/></param>
    /// <returns>True in case <paramref name="type"/> is a <see cref="ApplicationResult{T}"/>, otherwise false.</returns>
    private static bool IsGenericApplicationResult(Type type, out Type? successResultType)
    {
        successResultType = null;
        if (!type.IsGenericType){ return false;}

        Type genericType = type.GetGenericTypeDefinition();
        if (genericType != typeof(ApplicationResult<>)) return false;

        successResultType = type.GetGenericArguments()[0];
        return true;
    }

    // Helper method to create ApplicationResult<T>.Failure using reflection
    private static object CreateFailureResult(Type resultType, ApplicationError error)
    {
        Type applicationResultType = typeof(ApplicationResult<>).MakeGenericType(resultType);
        MethodInfo failureMethod = applicationResultType.GetMethod("Failure", BindingFlags.Public | BindingFlags.Static)!;
        return failureMethod.Invoke(null, [error])!;
    }
}
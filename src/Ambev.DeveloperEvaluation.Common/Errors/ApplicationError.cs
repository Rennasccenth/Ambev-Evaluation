using Ambev.DeveloperEvaluation.Common.Validation;
using FluentValidation.Results;

namespace Ambev.DeveloperEvaluation.Common.Errors;

public record ApplicationError(string Code, string Message)
{
    public string Code { get; } = Code;
    public string Message { get; } = Message;

    public static BadRequestError BadRequestError(string? message = null) =>
        message is null
            ? new BadRequestError()
            : new BadRequestError(message);

    public static ValidationError ValidationError(string? message = null) =>
        message is null
            ? new ValidationError()
            : new ValidationError(message);

    public static ValidationError ValidationError(IEnumerable<ValidationFailure> validationFailures) 
        => new(validationFailures);

    public static ValidationError ValidationError(IEnumerable<ValidationErrorDetail> validationErrorDetails) 
        => new(validationErrorDetails);
    public static ValidationError ValidationError(ValidationErrorDetail validationErrorDetail) 
        => new([validationErrorDetail]);
    
    public static DuplicatedResourceError DuplicatedResourceError(string? message = null) =>
        message is null
            ? new DuplicatedResourceError()
            : new DuplicatedResourceError(message);

    public static InvalidArgumentError InvalidArgumentError(string? message = null) =>
        message is null
            ? new InvalidArgumentError()
            : new InvalidArgumentError(message);

    public static NotFoundError NotFoundError(string? message = null) =>
        message is null
            ? new NotFoundError()
            : new NotFoundError(message);

    public static PermissionDeniedError PermissionDeniedError(string? message = null) =>
        message is null
            ? new PermissionDeniedError()
            : new PermissionDeniedError(message);

    public static UnauthorizedAccessError UnauthorizedAccessError(string? message = null) =>
        message is null
            ? new UnauthorizedAccessError()
            : new UnauthorizedAccessError(message);
}
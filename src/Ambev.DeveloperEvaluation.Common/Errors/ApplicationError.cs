using Ambev.DeveloperEvaluation.Common.Validation;
using FluentValidation.Results;

namespace Ambev.DeveloperEvaluation.Common.Errors;

public record ApplicationError
{
    
    protected ApplicationError(string Code, string Message)
    {
        this.Code = Code;
        this.Message = Message;
    }

    public string Code { get; }
    public string Message { get; }

    public static implicit operator ApplicationError (ValidationFailure validationFailure) => new ValidationError([validationFailure]);
    public static implicit operator ApplicationError (List<ValidationFailure> validationFailures) => new ValidationError(validationFailures);
    
    public static implicit operator ApplicationError (ValidationErrorDetail validationErrorDetail) => new ValidationError([validationErrorDetail]);
    public static implicit operator ApplicationError (List<ValidationErrorDetail> validationErrorDetails) => new ValidationError(validationErrorDetails);

    public static ValidationError ValidationError(ValidationErrorDetail validationErrorDetail) => validationErrorDetail;
    public static ValidationError ValidationError(ValidationResult validationResult) => new(validationResult.Errors);
    public static ValidationError ValidationError(ValidationResult[] validationResults) 
        => new(validationResults.SelectMany(validationResult => validationResult.Errors).ToList());

    public static UnprocessableError UnprocessableError(string? message = null) =>
        message is null
            ? new UnprocessableError()
            : new UnprocessableError(message);
    
    public static BadRequestError BadRequestError(string? message = null) =>
        message is null
            ? new BadRequestError()
            : new BadRequestError(message);

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
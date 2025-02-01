using Ambev.DeveloperEvaluation.Common.Validation;
using FluentValidation.Results;

namespace Ambev.DeveloperEvaluation.Common.Errors;

public sealed record ValidationError : ApplicationError
{
    public Dictionary<string, string[]> ErrorsDictionary { get; init; } = [];
    internal ValidationError() : base(Code: "VALIDATION_ERROR", Message: "One or more validation errors occurred.") { }
    internal ValidationError(string message) : base(Code: "VALIDATION_ERROR", message) { }

    public ValidationError(List<ValidationFailure> errors)
        : base("VALIDATION_ERROR", "One or more validation errors occurred.")
    {
        ErrorsDictionary = errors
            .Select(failure => (ValidationErrorDetail)failure)
            .GroupBy(detail => detail.Error)
            .ToDictionary(
                group => group.Key,
                group => group.Select(detail => detail.Detail).ToArray()
            );
    }

    public ValidationError(List<ValidationErrorDetail> errorsDetails)
        : base("VALIDATION_ERROR", "One or more validation errors occurred.")
    {
        ErrorsDictionary = errorsDetails
            .GroupBy(detail => detail.Error)
            .ToDictionary(
                group => group.Key,
                group => group.Select(detail => detail.Detail).ToArray()
            );
    }

    public static implicit operator ValidationError (ValidationFailure[] validationFailures) => new (validationFailures.ToList());
    public static implicit operator ValidationError (ValidationFailure validationFailure) => new ([validationFailure]);
    public static implicit operator ValidationError (List<ValidationFailure> validationFailures) => new (validationFailures);
    
    public static implicit operator ValidationError (ValidationErrorDetail validationErrorDetail) => new ([validationErrorDetail]);
    public static implicit operator ValidationError (List<ValidationErrorDetail> validationErrorDetails) => new (validationErrorDetails);
}
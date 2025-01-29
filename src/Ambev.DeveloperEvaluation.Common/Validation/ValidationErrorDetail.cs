using FluentValidation.Results;

namespace Ambev.DeveloperEvaluation.Common.Validation;

public class ValidationErrorDetail
{
    public string Error { get; init; }
    public string Detail { get; init; }

    public ValidationErrorDetail(string error, string detail)
    {
        Error = error;
        Detail = detail;
    }

    public static explicit operator ValidationErrorDetail(ValidationFailure validationFailure)
    {
        return new ValidationErrorDetail(validationFailure.PropertyName, validationFailure.ErrorMessage);
    }
}

using System.Collections.Immutable;
using Ambev.DeveloperEvaluation.Common.Validation;
using FluentValidation.Results;

namespace Ambev.DeveloperEvaluation.Common.Errors;

public sealed record ValidationError : ApplicationError
{
    internal ValidationError() : base(Code: "VALIDATION_ERROR", Message: "Bad request format or data provided.") { }
    internal ValidationError(string message) : base(Code: "VALIDATION_ERROR", message) { }

    internal ValidationError(IEnumerable<ValidationFailure> errors) 
        : base(
            Code: "VALIDATION_ERROR", 
            Message: errors
                .Select(failure => (ValidationErrorDetail)failure)
                .GroupBy(detail => detail.Error)
                .ToImmutableDictionary(
                    group => group.Key,
                    group => group.Select(detail => detail.Detail)
                ).ToString() ?? "") { }
    
    internal ValidationError(IEnumerable<ValidationErrorDetail> errorsDetails) 
        : base(
            Code: "VALIDATION_ERROR", 
            Message: errorsDetails
                .GroupBy(detail => detail.Error)
                .ToImmutableDictionary(
                    group => group.Key,
                    group => group.Select(detail => detail.Detail)
                ).ToString() ?? "") { }
}
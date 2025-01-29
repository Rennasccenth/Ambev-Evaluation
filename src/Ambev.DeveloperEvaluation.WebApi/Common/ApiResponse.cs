using Ambev.DeveloperEvaluation.Common.Validation;
using FluentValidation.Results;

namespace Ambev.DeveloperEvaluation.WebApi.Common;

public class ApiResponse
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public IEnumerable<ValidationErrorDetail> Errors { get; set; } = [];

    public ApiResponse() { }

    public ApiResponse(IEnumerable<ValidationFailure> failures)
    {
        Success = false;
        Message = "A validation error has occurred.";
        Errors = failures.Select(fail => (ValidationErrorDetail)fail);
    }
}

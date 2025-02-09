namespace Ambev.DeveloperEvaluation.Common.Errors;

public sealed record UnprocessableError : ApplicationError
{
    internal UnprocessableError() : base(Code: "UNPROCESSABLE_ERROR", Message: "Cannot proceed with this process.") { }
    internal UnprocessableError(string message) : base(Code: "UNPROCESSABLE_ERROR", message) { }
}
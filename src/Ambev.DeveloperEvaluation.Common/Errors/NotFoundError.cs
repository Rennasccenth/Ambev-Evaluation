namespace Ambev.DeveloperEvaluation.Common.Errors;

public sealed record NotFoundError : ApplicationError
{
    internal NotFoundError() : base(Code: "NOT_FOUND", Message: "Resource not found.") { }
    internal NotFoundError(string message) : base(Code: "NOT_FOUND", message) { }
}
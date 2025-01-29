namespace Ambev.DeveloperEvaluation.Common.Errors;

public sealed record BadRequestError : ApplicationError
{
    internal BadRequestError() : base(Code: "BAD_REQUEST", Message: "Bad request format or data provided.") { }
    internal BadRequestError(string message) : base(Code: "BAD_REQUEST", message) { }
}
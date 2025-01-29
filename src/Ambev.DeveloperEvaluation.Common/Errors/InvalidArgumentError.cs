namespace Ambev.DeveloperEvaluation.Common.Errors;

public sealed record InvalidArgumentError : ApplicationError
{
    internal InvalidArgumentError() : base(Code: "INVALID_ARGUMENT", Message: "One or more arguments are invalid.") { }
    internal InvalidArgumentError(string message) : base(Code: "INVALID_ARGUMENT", message) { }
}
namespace Ambev.DeveloperEvaluation.Common.Errors;

public sealed record UnauthorizedAccessError : ApplicationError
{
    internal UnauthorizedAccessError() : base(Code: "UNAUTHORIZED_ACCESS", Message: "Client does not have access rights to this resource.") { }
    internal UnauthorizedAccessError(string message) : base(Code: "UNAUTHORIZED_ACCESS", message) { }
}
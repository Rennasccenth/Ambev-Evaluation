namespace Ambev.DeveloperEvaluation.Common.Errors;

public sealed record DuplicatedResourceError : ApplicationError
{
    internal DuplicatedResourceError() : base(Code: "DUPLICATED_RESOURCE", Message: "Resource already exists.") { }
    internal DuplicatedResourceError(string message) : base(Code: "DUPLICATED_RESOURCE", message) { }
}
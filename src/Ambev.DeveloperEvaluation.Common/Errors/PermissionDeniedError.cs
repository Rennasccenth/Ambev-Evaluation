namespace Ambev.DeveloperEvaluation.Common.Errors;

public sealed record PermissionDeniedError : ApplicationError
{
    internal PermissionDeniedError() : base(Code: "PERMISSION_DENIED", Message: "User does not have permission to access this resource.") { }
    internal PermissionDeniedError(string message) : base(Code: "PERMISSION_DENIED", message) { }
}
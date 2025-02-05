namespace Ambev.DeveloperEvaluation.Common.Security;

public interface IUserContext
{
    Guid? UserId { get; }
    bool IsAuthenticated { get; }
}
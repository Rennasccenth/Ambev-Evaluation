namespace Ambev.DeveloperEvaluation.Common.Security;

public interface IUserContext
{
    Guid? UserUuid { get; }
    bool IsAuthenticated { get; }
}
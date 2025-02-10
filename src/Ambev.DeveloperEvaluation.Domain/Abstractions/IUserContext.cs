using Ambev.DeveloperEvaluation.Domain.Aggregates.Users.Enums;

namespace Ambev.DeveloperEvaluation.Domain.Abstractions;

public interface IUserContext
{
    Guid? UserId { get; }
    UserRole? UserRole { get; }
    bool IsAuthenticated { get; }
}
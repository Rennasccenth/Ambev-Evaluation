using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.ValueObjects;

namespace Ambev.DeveloperEvaluation.Application.Users.Commands.UpdateUser;

public sealed record UpdateUserCommandResult
{
    public required Guid Id { get; init; }
    public required Email Email { get; init; }
    public required string Username { get; init; }
    public required Password Password { get; init; }
    public required NameDto Name { get; init; }
    public required Address Address { get; init; }
    public required Phone Phone { get; init; }
    public required UserStatus Status { get; init; }
    public required UserRole Role { get; init; }
}
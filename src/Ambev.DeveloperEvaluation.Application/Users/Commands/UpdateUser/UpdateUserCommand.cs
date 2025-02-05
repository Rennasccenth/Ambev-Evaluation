using Ambev.DeveloperEvaluation.Common.Results;
using Ambev.DeveloperEvaluation.Domain.Aggregates.Users.Enums;
using Ambev.DeveloperEvaluation.Domain.ValueObjects;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Users.Commands.UpdateUser;

public sealed class UpdateUserCommand : IRequest<ApplicationResult<UpdateUserCommandResult>>
{
    public required Guid Id { get; init; }
    public required string Username { get; init; }
    public required Password Password { get; init; }
    public required Phone Phone { get; init; }
    public required Email Email { get; init; }
    public required Address Address { get; init; }
    public required UserStatus Status { get; init; }
    public required UserRole Role { get; init; }
}
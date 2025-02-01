using Ambev.DeveloperEvaluation.Common.Results;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.ValueObjects;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Users.CreateUser;

public sealed class CreateUserCommand : IRequest<CommandResult<CreateUserResult>>
{
    public required string Firstname { get; init; }
    public required string Lastname { get; init; }
    public required string City { get; init; }
    public required string Street { get; init; }
    public required int Number { get; init; }
    public required string ZipCode { get; init; }
    public required string Latitude { get; init; }
    public required string Longitude { get; init; }
    public required string Username { get; init; }
    public required Password Password { get; init; }
    public required Phone Phone { get; init; }
    public required Email Email { get; init; }
    public required UserStatus Status { get; init; }
    public required UserRole Role { get; init; }
}
using Ambev.DeveloperEvaluation.Domain.Enums;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Users.CreateUser;

public sealed class CreateUserCommand : IRequest<CreateUserResult>
{
    public required string City { get; init; }
    public required string Street { get; init; }
    public required int Number { get; init; }
    public required string ZipCode { get; init; }
    public required string Latitude { get; init; }
    public required string Longitude { get; init; }
    public required string Username { get; init; }
    public required string Password { get; init; }
    public required string Phone { get; init; }
    public required string Email { get; init; }
    public required UserStatus Status { get; init; }
    public required UserRole Role { get; init; }
}
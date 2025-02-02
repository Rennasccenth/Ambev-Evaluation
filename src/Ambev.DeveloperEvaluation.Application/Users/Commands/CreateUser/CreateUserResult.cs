using Ambev.DeveloperEvaluation.Domain.Enums;

namespace Ambev.DeveloperEvaluation.Application.Users.Commands.CreateUser;

public sealed class CreateUserResult
{
    public required Guid Id { get; init; }
    public required string Email { get; init; }
    public required string Username { get; init; }
    public required string Password { get; init; }
    public required CreatedName Name { get; init; }
    public required CreatedAddress Address { get; init; }
    public required string Phone { get; init; }
    public required UserStatus Status { get; init; }
    public required UserRole Role { get; init; }
}

public sealed record CreatedAddress
{
    public required string City { get; init; }
    public required string Street { get; init; }
    public required int Number { get; init; }
    public required string Zipcode { get; init; }
    public required CreatedGeolocation Geolocation { get; init; }
}

public sealed record CreatedGeolocation
{
    public required string Latitude { get; init; }
    public required string Longitude { get; init; }
}

public class CreatedName
{
    public required string Firstname { get; init; }
    public required string Lastname { get; init; }
}
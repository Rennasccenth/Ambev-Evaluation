using Ambev.DeveloperEvaluation.Domain.Enums;

namespace Ambev.DeveloperEvaluation.Application.Users.GetUser;

public sealed class GetUserResult
{
    public required Guid Id { get; init; }
    public required string Email { get; init; }
    public required string Username { get; init; }
    public required string Password { get; init; }
    public required NameDto Name { get; init; }
    public required AddressDto Address { get; init; }
    public required string Phone { get; init; }
    public required UserStatus Status { get; init; }
    public required UserRole Role { get; init; }
}

public sealed record AddressDto
{
    public required string City { get; init; }
    public required string Street { get; init; }
    public required int Number { get; init; }
    public required string Zipcode { get; init; }
    public required GeolocationDto Geolocation { get; init; }
}

public sealed record GeolocationDto
{
    public required string Latitude { get; init; }
    public required string Longitude { get; init; }
}

public class NameDto
{
    public required string Firstname { get; init; }
    public required string Lastname { get; init; }
}

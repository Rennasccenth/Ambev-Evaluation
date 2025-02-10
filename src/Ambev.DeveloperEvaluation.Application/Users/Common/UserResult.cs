using Ambev.DeveloperEvaluation.Domain.Aggregates.Users.Enums;

namespace Ambev.DeveloperEvaluation.Application.Users.Common;

public sealed class UserResult
{
    public required Guid Id { get; init; }
    public required string Email { get; init; }
    public required string Username { get; init; }
    public required string Password { get; init; }
    public required NameResult Name { get; init; }
    public required AddressResult Address { get; init; }
    public required string Phone { get; init; }
    public required UserStatus Status { get; init; }
    public required UserRole Role { get; init; }
}

public sealed class NameResult
{
    public required string Firstname { get; init; }
    public required string Lastname { get; init; }
}

public sealed class AddressResult
{
    public required string City { get; init; }
    public required string Street { get; init; }
    public required int Number { get; init; }
    public required string Zipcode { get; init; }
    public required GeolocationResult Geolocation { get; init; }

}

public sealed class GeolocationResult
{
    public required string Lat { get; init; }
    public required string Long { get; init; }
}
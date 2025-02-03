namespace Ambev.DeveloperEvaluation.WebApi.Features.Users.Queries.GetUser;

public sealed class GetUserResponse
{
    public required Guid Id { get; init; }
    public required string Email { get; init; }
    public required string Username { get; init; }
    public required string Password { get; init; }
    public required NameDto Name { get; init; }
    public required AddressDto Address { get; init; }
    public required string Phone { get; init; }
    public required string Status { get; init; }
    public required string Role { get; init; }
}

public sealed record AddressDto
{
    public string City { get; init; }
    public string Street { get; init; }
    public int Number { get; init; }
    public string Zipcode { get; init; }
    public GeolocationDto Geolocation { get; init; }
}

public sealed record GeolocationDto
{
    public string Lat { get; init; }
    public string Long { get; init; }
}

public class NameDto
{
    public string Firstname { get; init; }
    public string Lastname { get; init; }
}

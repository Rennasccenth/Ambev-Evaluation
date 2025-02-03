namespace Ambev.DeveloperEvaluation.WebApi.Features.Users.Queries.GetUser;

public sealed class GetUserResponse
{
    public Guid Id { get; init; }
    public string Email { get; init; } = null!;
    public string Username { get; init; } = null!;
    public string Password { get; init; } = null!;
    public NameDto Name { get; init; } = null!;
    public AddressDto Address { get; init; } = null!;
    public string Phone { get; init; } = null!;
    public string Status { get; init; } = null!;
    public string Role { get; init; } = null!;
}

public sealed record AddressDto
{
    public string City { get; init; } = null!;
    public string Street { get; init; } = null!;
    public int Number { get; init; }
    public string Zipcode { get; init; } = null!;
    public GeolocationDto Geolocation { get; init; } = null!;
}

public sealed record GeolocationDto
{
    public string Lat { get; init; } = null!;
    public string Long { get; init; } = null!;
}

public class NameDto
{
    public string Firstname { get; init; } = null!;
    public string Lastname { get; init; } = null!;
}

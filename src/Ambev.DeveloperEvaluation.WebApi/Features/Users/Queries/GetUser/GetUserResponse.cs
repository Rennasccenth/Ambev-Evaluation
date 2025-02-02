namespace Ambev.DeveloperEvaluation.WebApi.Features.Users.Queries.GetUser;

public sealed class GetUserResponse
{
    public Guid Id { get; init; }
    public string Email { get; init; }
    public string Username { get; init; }
    public string Password { get; init; }
    public NameDto Name { get; init; }
    public AddressDto Address { get; init; }
    public string Phone { get; init; }
    public string Status { get; init; }
    public string Role { get; init; }
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

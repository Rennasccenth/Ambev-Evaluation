namespace Ambev.DeveloperEvaluation.WebApi.Features.Users.Commands.CreateUser;

public sealed class CreateUserRequest
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public NameDto Name { get; set; } = null!;
    public AddressDto Address { get; set; } = null!;
    public string Status { get; set; } = "";
    public string Role { get; set; } = "";
}

public record NameDto
{
    public required string Firstname { get; init; }
    public required string Lastname { get; init; }
}
    

public record AddressDto
{
    public required string City { get; init; }
    public required string Street { get; init; }
    public required int Number { get; init; }
    public required string Zipcode { get; init; }
    public required GeolocationDto Geolocation { get; init; }
}

public record GeolocationDto
{
    public required string Lat { get; init; }
    public required string Long { get; init; }
}

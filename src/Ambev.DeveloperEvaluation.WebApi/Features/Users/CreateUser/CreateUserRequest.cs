using Ambev.DeveloperEvaluation.Domain.Enums;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Users.CreateUser;

public sealed class CreateUserRequest
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public NameDto Name { get; set; } = null!;
    public AddressDto Address { get; set; } = null!;
    public UserStatus Status { get; set; }
    public UserRole Role { get; set; }
}

public record NameDto(
    string Firstname,
    string Lastname
);

public record AddressDto(
    string City,
    string Street,
    string Number,
    string Zipcode,
    GeolocationDto Geolocation
);

public record GeolocationDto(
    string Lat,
    string Long
);


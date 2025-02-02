namespace Ambev.DeveloperEvaluation.WebApi.Features.Users.Commands.UpdateUser;

public sealed class UpdateUserRequest
{
    public required Guid Id { get; init; }
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public UpdatingAddress Address { get; set; } = null!;
    public string Status { get; set; } = "";
    public string Role { get; set; } = "";
}

public sealed record UpdatingAddress
{
    public required string City { get; init; }
    public required string Street { get; init; }
    public required int Number { get; init; }
    public required string Zipcode { get; init; }
    public required NewGeolocation Geolocation { get; init; }
}

public sealed record NewGeolocation
{
    public required string Lat { get; init; }
    public required string Long { get; init; }
}

public sealed record NameDto
{
    public required string Firstname { get; init; }
    public required string Lastname { get; init; }
}

public sealed record UpdateUserResponse
{
    public required Guid Id { get; init; }
    public required string Email { get; init; }
    public required string Username { get; init; }
    public required string Password { get; init; }
    public required NameDto Name { get; init; }
    public required UpdatingAddress Address { get; init; }
    public required string Phone { get; init; }
    public required string Status { get; init; }
    public required string Role { get; init; }
}
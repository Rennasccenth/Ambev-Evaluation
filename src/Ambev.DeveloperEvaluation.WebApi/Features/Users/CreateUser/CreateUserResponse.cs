namespace Ambev.DeveloperEvaluation.WebApi.Features.Users.CreateUser;

public sealed class CreateUserResponse
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

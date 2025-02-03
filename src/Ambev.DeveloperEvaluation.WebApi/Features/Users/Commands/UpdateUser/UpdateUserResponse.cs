namespace Ambev.DeveloperEvaluation.WebApi.Features.Users.Commands.UpdateUser;

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
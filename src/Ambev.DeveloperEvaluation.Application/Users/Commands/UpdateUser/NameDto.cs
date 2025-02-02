namespace Ambev.DeveloperEvaluation.Application.Users.Commands.UpdateUser;

public sealed record NameDto
{
    public required string Firstname { get; init; }
    public required string Lastname { get; init; }
}
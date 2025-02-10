namespace Ambev.DeveloperEvaluation.WebApi.Features.Auth.Commands.AuthenticateUser;

public sealed class AuthenticateUserResponse
{
    public required string Token { get; init; }
}

using Ambev.DeveloperEvaluation.Domain.ValueObjects;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Auth.AuthenticateUser;

public class AuthenticateUserRequest
{
    public Email Email { get; set; } = string.Empty;

    public Password Password { get; set; } = string.Empty;
}

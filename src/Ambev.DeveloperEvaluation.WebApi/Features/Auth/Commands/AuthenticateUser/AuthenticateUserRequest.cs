﻿namespace Ambev.DeveloperEvaluation.WebApi.Features.Auth.Commands.AuthenticateUser;

public class AuthenticateUserRequest
{
    public string Email { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;
}

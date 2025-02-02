using Ambev.DeveloperEvaluation.Common.Results;
using Ambev.DeveloperEvaluation.Domain.ValueObjects;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Auth.AuthenticateUser;

public sealed class AuthenticateUserCommand : IRequest<ApplicationResult<AuthenticateUserResult>>
{
    public Email Email { get; set; } = string.Empty;
    public Password Password { get; set; } = string.Empty;
}

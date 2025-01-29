using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Auth.AuthenticateUser;

internal sealed class AuthenticateUserValidator : AbstractValidator<AuthenticateUserCommand>
{
    internal AuthenticateUserValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(x => x.Password)
            .NotEmpty()
            .MinimumLength(6);
    }
}
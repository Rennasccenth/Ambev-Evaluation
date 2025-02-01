using Ambev.DeveloperEvaluation.Domain.ValueObjects;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Auth.AuthenticateUser;

public sealed class AuthenticateUserValidator : AbstractValidator<AuthenticateUserCommand>
{
    public AuthenticateUserValidator(
        IValidator<Email> emailValidator,
        IValidator<Password> passwordValidator)
    {
        RuleFor(x => x.Email)
            .SetValidator(emailValidator);

        RuleFor(x => x.Password)
            .SetValidator(passwordValidator);
    }
}
using Ambev.DeveloperEvaluation.Domain.ValueObjects;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Auth.AuthenticateUser;

public class AuthenticateUserRequestValidator : AbstractValidator<AuthenticateUserRequest>
{
    public AuthenticateUserRequestValidator(
        IValidator<Email> emailValidator,
        IValidator<Password> passwordValidator)
    {
        RuleFor(x => x.Email)
            .SetValidator(emailValidator);

        RuleFor(x => x.Password)
            .SetValidator(passwordValidator);
    }
}

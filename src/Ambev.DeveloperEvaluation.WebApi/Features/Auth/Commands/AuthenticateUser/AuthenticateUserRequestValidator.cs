using Ambev.DeveloperEvaluation.Domain.ValueObjects;
using FluentValidation;
using FluentValidation.Results;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Auth.Commands.AuthenticateUser;

public class AuthenticateUserRequestValidator : AbstractValidator<AuthenticateUserRequest>
{
    public AuthenticateUserRequestValidator(
        IValidator<Email> emailValidator,
        IValidator<Password> passwordValidator)
    {
        RuleFor(x => x.Email)
            .Custom((email, context) =>
            {
                Email emailObj = email;
                ValidationResult validationResult = emailValidator.Validate(emailObj);
                foreach (ValidationFailure? failure in validationResult.Errors)
                {
                    context.AddFailure(failure);
                }
            });

        RuleFor(x => x.Password)
            .Custom((password, context) =>
            {
                Password passwordObj = password;
                ValidationResult validationResult = passwordValidator.Validate(passwordObj);
                foreach (ValidationFailure? failure in validationResult.Errors)
                {
                    context.AddFailure(failure);
                }
            });
    }
}

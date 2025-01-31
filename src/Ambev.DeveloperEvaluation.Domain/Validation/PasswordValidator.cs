using Ambev.DeveloperEvaluation.Domain.ValueObjects;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Domain.Validation;

public class PasswordValidator : AbstractValidator<Password>
{
    public PasswordValidator()
    {
        RuleFor(password => password)
            .Custom((password, context) =>
            {
                if (password.IsValid)
                {
                    return;
                }
                foreach (var error in password.ValidationErrors)
                {
                    context.AddFailure(error);
                }
            });
    }
}
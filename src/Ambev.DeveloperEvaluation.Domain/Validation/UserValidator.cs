using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.ValueObjects;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Domain.Validation;

public class UserValidator : AbstractValidator<User>
{
    public UserValidator(
        IValidator<Email> emailValidator,
        IValidator<Phone> phoneValidator,
        IValidator<Password> passwordValidator)
    {
        RuleFor(user => user.Email)
            .SetValidator(emailValidator);
        RuleFor(user => user.Password)
            .SetValidator(passwordValidator);
        RuleFor(user => user.Phone)
            .SetValidator(phoneValidator);

        RuleFor(user => user.Username)
            .NotEmpty()
            .MinimumLength(3).WithMessage("Username must be at least 3 characters long.")
            .MaximumLength(50).WithMessage("Username cannot be longer than 50 characters.");

        RuleFor(user => user.Status).IsInEnum();
        RuleFor(user => user.Role).IsInEnum();
    }
}

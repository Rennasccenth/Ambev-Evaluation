using Ambev.DeveloperEvaluation.Domain.ValueObjects;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Users.Commands.CreateUser;

public sealed class CreateUserValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserValidator(
        IValidator<Email> emailValidator,
        IValidator<Password> passwordValidator,
        IValidator<Phone> phoneValidator)
    {
        RuleFor(user => user.Email)
            .SetValidator(emailValidator);
        RuleFor(user => user.Password)
            .SetValidator(passwordValidator);
        RuleFor(user => user.Phone)
            .SetValidator(phoneValidator);

        RuleFor(user => user.Username).NotEmpty().Length(3, 50);
        RuleFor(user => user.Firstname).NotEmpty().Length(2, 50);
        RuleFor(user => user.Lastname).NotEmpty().Length(2, 50);
        RuleFor(user => user.City).NotEmpty().Length(3, 50);
        RuleFor(user => user.Street).NotEmpty().Length(3, 70);
        RuleFor(user => user.Number).NotEmpty().GreaterThan(-1);
        RuleFor(user => user.ZipCode).NotEmpty().Length(3, 50);
        RuleFor(user => user.Latitude).NotEmpty().Length(3, 50);
        RuleFor(user => user.Longitude).NotEmpty().Length(3, 50);
        RuleFor(user => user.Role);
        RuleFor(user => user.Status);
    }
}
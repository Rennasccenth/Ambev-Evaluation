using Ambev.DeveloperEvaluation.Domain.Validation;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Users.CreateUser;

public sealed class CreateUserValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserValidator()
    {
        RuleFor(user => user.Email)
            .SetValidator(new EmailValidator());
        RuleFor(user => user.Password)
            .SetValidator(new PasswordValidator());
        RuleFor(user => user.Phone)
            .SetValidator(new PhoneValidator());

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
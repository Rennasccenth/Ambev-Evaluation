using Ambev.DeveloperEvaluation.Domain.ValueObjects;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Users.Commands.UpdateUser;

public sealed class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
{
    public UpdateUserCommandValidator(
        IValidator<Password> passwordValidator,
        IValidator<Phone> phoneValidator,
        IValidator<Email> emailValidator,
        IValidator<Address> addressValidator)
    {
        RuleFor(command => command.Id).NotEmpty();
        RuleFor(command => command.Username).NotEmpty().Length(3, 50);
        RuleFor(command => command.Password).SetValidator(passwordValidator);
        RuleFor(command => command.Phone).SetValidator(phoneValidator);
        RuleFor(command => command.Email).SetValidator(emailValidator);
        RuleFor(command => command.Address).SetValidator(addressValidator);
        RuleFor(command => command.Role).NotNull();
        RuleFor(command => command.Status).NotNull();
    }
}
using Ambev.DeveloperEvaluation.Domain.Aggregates.Users.Enums;
using Ambev.DeveloperEvaluation.Domain.ValueObjects;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Users.Commands.UpdateUser;

public sealed class UpdateUserRequestValidator : AbstractValidator<UpdateUserRequest>
{
    public UpdateUserRequestValidator(
        IValidator<Email> emailValidator,
        IValidator<Phone> phoneValidator,
        IValidator<Password> passwordValidator)
    {
        RuleFor(user => (Email)user.Email)
            .SetValidator(emailValidator);
        RuleFor(user =>(Password)user.Password)
            .SetValidator(passwordValidator);
        RuleFor(user => (Phone)user.Phone)
            .SetValidator(phoneValidator);
        
        RuleFor(user => user.Username)
            .NotNull()
            .NotEmpty().Length(3, 50);
        
        RuleFor(user => user.Address.City)
            .NotNull()
            .NotEmpty()
            .Length(3, 50);
        RuleFor(user => user.Address.Street)
            .NotNull()
            .NotEmpty()
            .Length(3, 70);
        
        RuleFor(user => user.Address.Number)
            .NotNull()
            .NotEmpty()
            .GreaterThan(-1);
        
        RuleFor(user => user.Address.Zipcode)
            .NotNull()
            .NotEmpty()
            .Length(3, 50);
        RuleFor(user => user.Address.Geolocation.Lat)
            .NotNull()
            .NotEmpty()
            .Length(3, 50);
        RuleFor(user => user.Address.Geolocation.Long)
            .NotNull()
            .NotEmpty()
            .Length(3, 50);
        
        RuleFor(user => user.Role).Must(stringRole => Enum.IsDefined(typeof(UserRole), stringRole));
        RuleFor(user => user.Status).Must(stringStatus => Enum.IsDefined(typeof(UserStatus), stringStatus));
    }
}
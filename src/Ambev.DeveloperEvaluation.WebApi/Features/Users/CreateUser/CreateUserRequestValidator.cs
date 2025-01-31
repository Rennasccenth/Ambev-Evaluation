using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.Validation;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Users.CreateUser;

/// <summary>
/// Validator for CreateUserRequest that defines validation rules for user creation.
/// </summary>
public sealed class CreateUserRequestValidator : AbstractValidator<CreateUserRequest>
{
    /// <summary>
    /// Initializes a new instance of the CreateUserRequestValidator with defined validation rules.
    /// </summary>
    /// <remarks>
    /// Validation rules include:
    /// - Email: Must be valid format (using EmailValidator)
    /// - Username: Required, length between 3 and 50 characters
    /// - Password: Must meet security requirements (using PasswordValidator)
    /// - Phone: Must match international format (+X XXXXXXXXXX)
    /// - Status: Cannot be Unknown
    /// - Role: Cannot be None
    /// </remarks>
    public CreateUserRequestValidator()
    {
        RuleFor(user => user.Email)
            .SetValidator(new EmailValidator());
        RuleFor(user => user.Password)
            .SetValidator(new PasswordValidator());
        RuleFor(user => user.Phone)
            .SetValidator(new PhoneValidator());
        
        RuleFor(user => user.Username)
            .NotNull()
            .NotEmpty().Length(3, 50);
        RuleFor(user => user.Name.Firstname)
            .NotNull()
            .NotEmpty().Length(2, 50);
        RuleFor(user => user.Name.Lastname)
            .NotNull()
            .NotEmpty().Length(2, 50);
        
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
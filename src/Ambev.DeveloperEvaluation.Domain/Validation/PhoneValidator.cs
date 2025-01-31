using Ambev.DeveloperEvaluation.Domain.ValueObjects;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Domain.Validation;

public class PhoneValidator : AbstractValidator<Phone>
{
    public PhoneValidator()
    {
        RuleFor(phone => phone)
            .Must(phone => phone.IsValid)
            .WithMessage("Invalid phone number.");

        RuleFor(phone => phone.ToString())
            .NotEmpty()
            .WithMessage("Phone number cannot be empty.")
            .MinimumLength(Phone.MinLength)
            .WithMessage($"Phone number must have at least {Phone.MinLength} characters.")
            .MaximumLength(Phone.MaxLength)
            .WithMessage($"Phone number must have at most {Phone.MaxLength} characters.");
    }
}

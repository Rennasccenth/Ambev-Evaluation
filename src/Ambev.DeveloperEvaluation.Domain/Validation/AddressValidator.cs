using Ambev.DeveloperEvaluation.Domain.ValueObjects;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Domain.Validation;

public sealed class AddressValidator : AbstractValidator<Address>
{
    public AddressValidator()
    {
        RuleFor(x => x.Street)
            .NotEmpty()
            .WithMessage("Street is required");
        
        RuleFor(x => x.Number)
            .GreaterThan(-1)
            .NotEmpty().WithMessage("Number is required");
        
        RuleFor(x => x.City)
            .NotEmpty()
            .WithMessage("City is required");
        RuleFor(x => x.ZipCode)
            .NotEmpty()
            .Custom((zipCode, context) =>
            {
                var zipCodeNumbers = new string(zipCode.Where(c => char.IsDigit(c)).ToArray());
                if (zipCodeNumbers.Length is < 12 and > 0)
                {
                    context.AddFailure("ZipCode is invalid");
                }
            });

        RuleFor(x => x.Latitude).NotEmpty().WithMessage("Latitude is required");
        RuleFor(x => x.Longitude).NotEmpty().WithMessage("Longitude is required");
    }
}
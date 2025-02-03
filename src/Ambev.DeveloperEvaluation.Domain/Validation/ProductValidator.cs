using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.ValueObjects;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Domain.Validation;

public sealed class ProductValidator : AbstractValidator<Product>
{
    public ProductValidator(IValidator<Rating> ratingValidator)
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.Price)
            .NotEmpty()
            .GreaterThan(0)
            .PrecisionScale(18, 2, false);

        RuleFor(x => x.Description)
            .NotEmpty()
            .MaximumLength(500);

        RuleFor(x => x.Category)
            .NotEmpty()
            .MaximumLength(80);

        RuleFor(x => x.Image)
            .NotEmpty();

        RuleFor(x => x.Rating)
            .NotNull()
            .SetValidator(ratingValidator);
    }
}
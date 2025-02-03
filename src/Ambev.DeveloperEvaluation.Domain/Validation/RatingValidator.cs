using Ambev.DeveloperEvaluation.Domain.ValueObjects;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Domain.Validation;

public sealed class RatingValidator : AbstractValidator<Rating>
{
    public RatingValidator()
    {
        RuleFor(x => x.Rate)
            .NotNull()
            .WithMessage("Rating value is required")
            .InclusiveBetween(0m, 5m)
            .WithMessage("Rating must be between 0 and 5");

        RuleFor(x => x.Count)
            .NotNull()
            .WithMessage("Rating count is required")
            .GreaterThanOrEqualTo(0)
            .WithMessage("Rating count must be non-negative");
    }
}
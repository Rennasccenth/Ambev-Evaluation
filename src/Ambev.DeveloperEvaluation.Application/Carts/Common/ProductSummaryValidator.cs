using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Carts.Common;

public sealed class ProductSummaryValidator : AbstractValidator<ProductSummary>
{
    public ProductSummaryValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty()
            .WithMessage("ProductId is required.");

        RuleFor(x => x.Quantity)
            .GreaterThan(0)
            .WithMessage("Quantity must be greater than 0.");
    }
}
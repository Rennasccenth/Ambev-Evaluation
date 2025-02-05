using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts.Common;

public sealed class ProductQuantifierValidator : AbstractValidator<ProductQuantifier>
{
    public ProductQuantifierValidator()
    {
        RuleFor(x => x.ProductId).NotEmpty();
        RuleFor(x => x.Quantity).GreaterThan(0);
    }
}
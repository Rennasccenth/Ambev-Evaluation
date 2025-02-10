using Ambev.DeveloperEvaluation.Application.Carts.Common;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Carts.Commands.UpdateCart;

public sealed class UpdateCartCommandValidator : AbstractValidator<UpdateCartCommand>
{
    public UpdateCartCommandValidator(IValidator<ProductSummary> productSummaryValidator)
    {
        RuleFor(x => x.CartId).NotEmpty();
        When(x => x.Products.Any(), () =>
        {
            RuleFor(x => x.Products).NotEmpty();
            RuleForEach(x => x.Products).SetValidator(productSummaryValidator);
        });
    }
}
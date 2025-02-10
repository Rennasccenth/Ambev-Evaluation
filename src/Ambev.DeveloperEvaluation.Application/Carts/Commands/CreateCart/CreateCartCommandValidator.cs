using Ambev.DeveloperEvaluation.Application.Carts.Common;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Carts.Commands.CreateCart;

public sealed class CreateCartCommandValidator : AbstractValidator<CreateCartCommand>
{
    public CreateCartCommandValidator(IValidator<ProductSummary> productSummaryValidator)
    {
        RuleFor(c => c.Date).NotEmpty();
        When(x => x.Products.Count != 0, () =>
        {
            RuleFor(c => c.Products).NotEmpty();
            RuleForEach(c => c.Products).SetValidator(productSummaryValidator);
        });
    }
}
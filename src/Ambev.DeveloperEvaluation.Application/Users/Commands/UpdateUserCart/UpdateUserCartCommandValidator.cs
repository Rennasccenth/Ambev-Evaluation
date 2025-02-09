using Ambev.DeveloperEvaluation.Application.Carts.Common;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Users.Commands.UpdateUserCart;

public sealed class UpdateUserCartCommandValidator : AbstractValidator<UpdateUserCartCommand>
{
    public UpdateUserCartCommandValidator(IValidator<ProductSummary> productSummaryValidator)
    {
        RuleFor(x => x.UserId).NotEmpty();
        When(x => x.Products.Any(), () =>
        {
            RuleFor(x => x.Products).NotEmpty();
            RuleForEach(x => x.Products).SetValidator(productSummaryValidator);
        });
    }
}
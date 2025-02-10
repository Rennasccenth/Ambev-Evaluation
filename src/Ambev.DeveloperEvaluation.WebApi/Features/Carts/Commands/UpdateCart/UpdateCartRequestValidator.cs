using Ambev.DeveloperEvaluation.WebApi.Features.Carts.Common;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts.Commands.UpdateCart;

public sealed class UpdateCartRequestValidator : AbstractValidator<UpdateCartRequest>
{
    public UpdateCartRequestValidator(IValidator<ProductQuantifier> productQuantifierValidator)
    {
        RuleFor(x => x.CartId).NotEmpty();
        When(x => x.Products.Any(), () =>
        {
            RuleForEach(x => x.Products).SetValidator(productQuantifierValidator);
        });
    }
}
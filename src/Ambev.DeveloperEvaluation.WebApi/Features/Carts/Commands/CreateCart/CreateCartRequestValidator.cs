using Ambev.DeveloperEvaluation.WebApi.Features.Carts.Common;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts.Commands.CreateCart;

public sealed class CreateCartRequestValidator : AbstractValidator<CreateCartRequest>
{
    public CreateCartRequestValidator(
        IValidator<ProductQuantifier> childValidator)
    {
        RuleFor(x => x.Date).NotEmpty();
        When(x => x.Products.Count != 0, () =>
        {
            RuleFor(x => x.Products).NotEmpty();
            RuleForEach(x => x.Products).SetValidator(childValidator);
        });
    }
}
using Ambev.DeveloperEvaluation.WebApi.Features.Carts.Common;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Users.Commands.UpdateUserCart;

public sealed class UpdateUserCartRequestValidator : AbstractValidator<UpdateUserCartRequest>
{
    public UpdateUserCartRequestValidator(IValidator<ProductQuantifier> childValidator)
    {
        RuleFor(x => x.UserId).NotEmpty();
        When(x => x.Products.Any(), () =>
        {
            RuleForEach(x => x.Products).SetValidator(childValidator);
        });
    }
}
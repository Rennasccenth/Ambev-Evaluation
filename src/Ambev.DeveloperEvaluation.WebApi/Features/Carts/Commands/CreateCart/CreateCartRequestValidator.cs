using Ambev.DeveloperEvaluation.Common.Security;
using Ambev.DeveloperEvaluation.WebApi.Features.Carts.Common;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts.Commands.CreateCart;

public sealed class CreateCartRequestValidator : AbstractValidator<CreateCartRequest>
{
    public CreateCartRequestValidator(
        IValidator<ProductQuantifier> childValidator,
        IUserContext userContext)
    {
        if (userContext.IsAuthenticated)
        {
            RuleFor(x => x.UserId).Must(x => x == userContext.UserId)
                .WithMessage("The provided UserId differs from current authenticated user.");  
        }
        else
        {
            RuleFor(x => x.UserId).NotEmpty();
        }

        RuleFor(x => x.Date).NotEmpty();
        When(x => x.Products.Count != 0, () =>
        {
            RuleFor(x => x.Products).NotEmpty();
            RuleForEach(x => x.Products).SetValidator(childValidator);
        });
    }
}
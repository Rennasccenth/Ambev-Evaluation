using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts.Commands.DeleteCart;

public sealed class DeleteCartRequestValidator : AbstractValidator<DeleteCartRequest>
{
    public DeleteCartRequestValidator()
    {
        RuleFor(x => x.CartId).NotEmpty();
    }
}
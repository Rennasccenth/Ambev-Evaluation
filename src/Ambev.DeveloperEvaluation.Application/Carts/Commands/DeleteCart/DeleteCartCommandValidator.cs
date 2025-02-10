using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Carts.Commands.DeleteCart;

public sealed class DeleteCartCommandValidator : AbstractValidator<DeleteCartCommand>
{
    public DeleteCartCommandValidator()
    {
        RuleFor(x => x.CartId).NotEmpty();
    }
}
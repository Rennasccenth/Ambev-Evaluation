using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Users.Commands.SellCartItems;

public sealed class SellCartItemsCommandValidator : AbstractValidator<SellCartItemsCommand>
{
    public SellCartItemsCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.BranchName).NotEmpty().MinimumLength(5);
    }
}
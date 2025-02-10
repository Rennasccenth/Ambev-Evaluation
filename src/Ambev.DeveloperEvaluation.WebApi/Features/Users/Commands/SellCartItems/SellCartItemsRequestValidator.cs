using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Users.Commands.SellCartItems;

public sealed class SellCartItemsRequestValidator : AbstractValidator<SellCartItemsRequest>
{
    public SellCartItemsRequestValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.BranchName).NotEmpty().MinimumLength(5);
    }
}
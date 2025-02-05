using Ambev.DeveloperEvaluation.Common.Security;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.Commands;

public sealed class CreateSaleCommandValidator : AbstractValidator<CreateSaleCommand>
{
    public CreateSaleCommandValidator(IUserContext userContext)
    {
        RuleFor(x => x.CartId).NotEmpty();
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.Branch).NotEmpty().MinimumLength(5);

        if (userContext.IsAuthenticated)
        {
            RuleFor(x => x.UserId)
                .Must(x => x == userContext.UserId)
                .WithMessage("You can only create a sale for yourself.");
        }
    }
}
using Ambev.DeveloperEvaluation.Application.Carts.Common;
using Ambev.DeveloperEvaluation.Common.Security;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Carts.Commands;

public sealed class CreateCartCommandValidator : AbstractValidator<CreateCartCommand>
{
    public CreateCartCommandValidator(IValidator<ProductSummary> productSummaryValidator, IUserContext userContext)
    {
        if (userContext.IsAuthenticated)
        {
            RuleFor(x => x.UserId).Must(x => x == userContext.UserId)
                .WithMessage("The provided UserId differs from current authenticated user.");  
        }

        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.Date).NotEmpty();
        When(x => x.Products.Count != 0, () =>
        {
            RuleFor(x => x.Products).NotEmpty();
            RuleForEach(x => x.Products).SetValidator(productSummaryValidator);
        });
    }
}
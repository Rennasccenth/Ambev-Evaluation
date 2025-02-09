using Ambev.DeveloperEvaluation.Application.Carts.Common;
using Ambev.DeveloperEvaluation.Common.Security;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Users.Commands.CreateUserCart;

public sealed class CreateUserCartCommandValidator : AbstractValidator<CreateUserCartCommand>
{
    public CreateUserCartCommandValidator(
        IValidator<ProductSummary> childValidator,
        IUserContext userContext)
    {
        if (userContext is { IsAuthenticated: true, UserId: not null })
        {
            RuleFor(x => x.UserId).Equal(userContext.UserId.Value);            
        }
        
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.Date).NotEmpty();
        When(x => x.Products.Count != 0, () =>
        {
            RuleForEach(x => x.Products).SetValidator(childValidator);
        });
    }
}
using Ambev.DeveloperEvaluation.Common.Security;
using Ambev.DeveloperEvaluation.Domain.Abstractions;
using Ambev.DeveloperEvaluation.WebApi.Features.Carts.Common;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Users.Commands.CreateUserCart;

public sealed class CreateUserCartRequestValidator : AbstractValidator<CreateUserCartRequest>
{
    public CreateUserCartRequestValidator(
        IValidator<ProductQuantifier> childValidator,
        IUserContext userContext)
    {
        RuleFor(x => x.Date).NotEmpty();
        if (userContext is { IsAuthenticated: true, UserId: not null })
        {
            RuleFor(x => x.UserId)
                .Equal(userContext.UserId.Value)
                .WithMessage("The provided UserId differs from current authenticated user.");    
        }
        
        When(x => x.Products.Count != 0, () =>
        {
            RuleFor(x => x.Products).NotEmpty();
            RuleForEach(x => x.Products).SetValidator(childValidator);
        });
    }
}
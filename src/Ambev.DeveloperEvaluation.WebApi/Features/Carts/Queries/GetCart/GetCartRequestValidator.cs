using Ambev.DeveloperEvaluation.Common.Security;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts.Queries.GetCart;

public class GetCartRequestValidator : AbstractValidator<GetCartRequest>
{
    public GetCartRequestValidator(IUserContext userContext)
    {
        if (userContext.IsAuthenticated)
        {
            RuleFor(x => x.UserId)
                .Must(x => x == userContext.UserId)
                .WithMessage("The provided UserId differs from current authenticated user.");  
        }
        
        RuleFor(x => x.UserId).NotEmpty().WithMessage("UserId is required");
    }
}
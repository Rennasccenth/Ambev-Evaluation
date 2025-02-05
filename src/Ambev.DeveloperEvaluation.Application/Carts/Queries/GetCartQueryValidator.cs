using Ambev.DeveloperEvaluation.Common.Security;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Carts.Queries;

public sealed class GetCartQueryValidator : AbstractValidator<GetCartQuery>
{
    public GetCartQueryValidator(IUserContext userContext)
    {
        if (userContext.IsAuthenticated)
        {
            RuleFor(x => x.UserId).Must(x => x == userContext.UserId)
                .WithMessage("The provided UserId differs from current authenticated user.");  
        }

        RuleFor(x => x.UserId).NotEmpty().WithMessage("UserId is required");
    }
}
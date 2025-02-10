using Ambev.DeveloperEvaluation.Common.Security;
using Ambev.DeveloperEvaluation.Domain.Abstractions;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Users.Queries.GetUserCart;

public sealed class GetUserCartQueryValidator : AbstractValidator<GetUserCartQuery>
{
    public GetUserCartQueryValidator(IUserContext context)
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("User Id is required");
        
        When(x => x.UserId != context.UserId, () =>
        {
            RuleFor(x => x.UserId)
                .Must(x => context.IsAuthenticated && x == context.UserId)
                .WithMessage("User does not have permission to access this resource");
        });
    }
}
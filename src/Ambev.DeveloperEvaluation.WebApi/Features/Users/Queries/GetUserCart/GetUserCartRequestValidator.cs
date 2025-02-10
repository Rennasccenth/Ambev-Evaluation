using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Users.Queries.GetUserCart;

public sealed class GetUserCartRequestValidator : AbstractValidator<GetUserCartRequest>
{
    public GetUserCartRequestValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
    }
}
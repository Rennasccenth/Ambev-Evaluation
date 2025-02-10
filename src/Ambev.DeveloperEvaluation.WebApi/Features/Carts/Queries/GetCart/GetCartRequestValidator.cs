using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts.Queries.GetCart;

public class GetCartRequestValidator : AbstractValidator<GetCartRequest>
{
    public GetCartRequestValidator()
    {
        RuleFor(x => x.CartId)
            .NotEmpty()
            .WithMessage("CartId is required");
    }
}
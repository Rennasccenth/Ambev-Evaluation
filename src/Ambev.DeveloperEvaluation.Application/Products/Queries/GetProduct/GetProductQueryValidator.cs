using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Products.Queries.GetProduct;

public sealed class GetProductQueryValidator : AbstractValidator<GetProductQuery>
{
    public GetProductQueryValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
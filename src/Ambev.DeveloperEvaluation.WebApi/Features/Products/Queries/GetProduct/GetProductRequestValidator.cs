using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.Queries.GetProduct;

public sealed class GetProductRequestValidator : AbstractValidator<GetProductRequest>
{
    public GetProductRequestValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
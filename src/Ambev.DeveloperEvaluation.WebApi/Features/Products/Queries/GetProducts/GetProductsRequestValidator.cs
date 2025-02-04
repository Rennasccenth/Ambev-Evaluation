using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.Queries.GetProducts;

public class GetProductsRequestValidator : AbstractValidator<GetProductsRequest>
{
    public GetProductsRequestValidator()
    {
        RuleFor(x => x.CurrentPage)
            .NotEmpty()
            .GreaterThanOrEqualTo(1);
        RuleFor(x => x.PageSize)
            .NotEmpty()
            .GreaterThanOrEqualTo(1)
            .LessThanOrEqualTo(100);

        RuleFor(x => x.FilterBy)
            .NotNull();

        RuleFor(x => x.FilterBy.Values)
            .Must(values => values.All(value => !string.IsNullOrEmpty(value)))
            .WithMessage("The query filter cannot contain empty or null values.");

        When(x => x.OrderBy != null, () =>
        {
            RuleFor(x => x.OrderBy).NotEmpty();
        });
    }
}
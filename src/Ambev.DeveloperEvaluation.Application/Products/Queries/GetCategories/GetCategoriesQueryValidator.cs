using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Products.Queries.GetCategories;

public sealed class GetCategoriesQueryValidator : AbstractValidator<GetCategoriesQuery>
{
    public GetCategoriesQueryValidator() { }
}
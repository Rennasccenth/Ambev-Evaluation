namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.Queries.GetCategories;

public sealed class GetCategoriesResponse : List<string>
{
    public GetCategoriesResponse(IEnumerable<string> categories) : base(categories) { }
}
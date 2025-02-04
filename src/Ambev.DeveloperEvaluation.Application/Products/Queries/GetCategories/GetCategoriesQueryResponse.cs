namespace Ambev.DeveloperEvaluation.Application.Products.Queries.GetCategories;

public sealed class GetCategoriesQueryResponse
{
    public List<string> Categories { get; }

    public GetCategoriesQueryResponse(List<string> categories)
    {
        Categories = categories;
    }
}
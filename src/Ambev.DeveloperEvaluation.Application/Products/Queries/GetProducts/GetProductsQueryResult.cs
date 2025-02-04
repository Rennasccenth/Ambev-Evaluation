using Ambev.DeveloperEvaluation.Domain.ValueObjects;

namespace Ambev.DeveloperEvaluation.Application.Products.Queries.GetProducts;

public class GetProductsQueryResult
{
    public List<GetProductsQueryResultItem> Products { get; init; } = [];
    public int CurrentPage { get; init; }
    public int TotalPages { get; init; }
    public int TotalItems { get; init; }
    public int PageSize { get; init; }
}

public sealed class GetProductsQueryResultItem
{
    public required Guid Id { get; init; }
    public required string Title { get; init; }
    public required decimal Price { get; init; }
    public required string Description { get; init; }
    public required string Category { get; init; }
    public required string Image { get; init; }
    public required Rating Rating { get; init; }
}


using Ambev.DeveloperEvaluation.Domain.ValueObjects;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.Commands.UpdateProduct;

public sealed class UpdateProductResponse
{
    public Guid Id { get; init; }
    public string Title { get; init; } = null!;
    public string Price { get; init; } = null!;
    public string Description { get; init; } = null!;
    public string Category { get; init; } = null!;
    public string Image { get; init; } = null!;
    public Rating Rating { get; init; } = null!;
}
using Ambev.DeveloperEvaluation.Domain.ValueObjects;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.Commands.CreateProduct;

public sealed class CreateProductRequest
{
    public string Title { get; init; } = null!;
    public decimal Price { get; init; }
    public string Description { get; init; } = null!;
    public string Category { get; init; } = null!;
    public string Image { get; init; } = null!;
    public Rating Rating { get; init; } = null!;
}
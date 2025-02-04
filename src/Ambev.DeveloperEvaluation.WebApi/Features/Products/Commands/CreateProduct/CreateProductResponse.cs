using Ambev.DeveloperEvaluation.Domain.ValueObjects;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.Commands.CreateProduct;

public sealed class CreateProductResponse
{
    public string Title { get; private set; } = null!;
    public string Price { get; private set; } = null!;
    public string Description { get; private set; } = null!;
    public string Category { get; private set; } = null!;
    public string Image { get; private set; } = null!;
    public Rating Rating { get; private set; } = null!;
}
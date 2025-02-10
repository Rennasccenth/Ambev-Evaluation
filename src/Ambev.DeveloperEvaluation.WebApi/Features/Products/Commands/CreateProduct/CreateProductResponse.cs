using Ambev.DeveloperEvaluation.Domain.ValueObjects;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.Commands.CreateProduct;

public sealed class CreateProductResponse
{
    public required Guid Id { get; init; }
    public required string Title { get; init; }
    public required string Price { get; init; }
    public required string Description { get; init; }
    public required string Category { get; init; }
    public required string Image { get; init; }
    public required Rating Rating { get; init; }
}
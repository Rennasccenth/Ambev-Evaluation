using Ambev.DeveloperEvaluation.Domain.ValueObjects;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.Commands.UpdateProduct;

public sealed class UpdateProductRequest
{
    public Guid Id { get; set; }
    public required string Title { get; init; }
    public required decimal Price { get; init; }
    public required string Description { get; init; }
    public required string Category { get; init; }
    public required string Image { get; init; }
    public required Rating Rating { get; init; }
}
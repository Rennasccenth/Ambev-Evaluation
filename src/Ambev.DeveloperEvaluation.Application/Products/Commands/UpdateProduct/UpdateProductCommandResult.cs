using Ambev.DeveloperEvaluation.Domain.ValueObjects;

namespace Ambev.DeveloperEvaluation.Application.Products.Commands.UpdateProduct;

public sealed class UpdateProductCommandResult
{
    public required Guid Id { get; init; }
    public required string Title { get; init; }
    public required decimal Price { get; init; }
    public required string Description { get; init; }
    public required string Category { get; init; }
    public required string Image { get; init; }
    public required Rating Rating { get; init; }
    public required DateTimeOffset CreatedAt { get; init; }
    public required DateTimeOffset UpdatedAt { get; init; }
}
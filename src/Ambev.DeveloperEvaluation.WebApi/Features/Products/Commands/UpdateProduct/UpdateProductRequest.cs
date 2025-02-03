using Ambev.DeveloperEvaluation.Domain.ValueObjects;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.Commands.UpdateProduct;

public sealed class UpdateProductRequest
{
    public Guid Id { get; set; }
    public string Title { get; init; }
    public decimal Price { get; init; }
    public string Description { get; init; }
    public string Category { get; init; }
    public string Image { get; init; }
    public Rating Rating { get; init; }
}
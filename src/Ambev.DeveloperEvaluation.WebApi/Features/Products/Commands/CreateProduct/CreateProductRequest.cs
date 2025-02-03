using System.ComponentModel.DataAnnotations;
using Ambev.DeveloperEvaluation.Domain.ValueObjects;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.Commands.CreateProduct;

public sealed class CreateProductRequest
{
    [Required]
    public required string Title { get; init; }

    [Required]
    [Range(0.01, double.MaxValue)]
    public required decimal Price { get; init; }

    [Required]
    public required string Description { get; init; }

    [Required]
    public required string Category { get; init; }

    [Required]
    public required string Image { get; init; }

    [Required]
    public required Rating Rating { get; init; }
}
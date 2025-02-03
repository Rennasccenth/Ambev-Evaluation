using System.ComponentModel.DataAnnotations;
using Ambev.DeveloperEvaluation.Domain.ValueObjects;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.Commands.CreateProduct;

public sealed class CreateProductRequest
{
    [Required]
    public string Title { get; init; }

    [Required]
    [Range(0.01, double.MaxValue)]
    public decimal Price { get; init; }

    [Required]
    public string Description { get; init; }

    [Required]
    public string Category { get; init; }

    [Required]
    public string Image { get; init; }

    [Required]
    public Rating Rating { get; init; }
}
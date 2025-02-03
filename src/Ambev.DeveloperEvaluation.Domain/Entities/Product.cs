using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.ValueObjects;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

public sealed class Product : BaseEntity
{
    internal Product() { }
    public Product(
        string title,
        decimal price,
        string description,
        string category,
        string image,
        Rating rating)
    {
        Title = title;
        Price = price;
        Description = description;
        Category = category;
        Image = image;
        Rating = rating;
    }

    public string Title { get; set; } = null!;
    public decimal Price { get; set; }
    public string Description { get; set; } = null!;
    public string Category { get; set; } = null!;
    public string Image { get; set; } = null!;
    public Rating Rating { get; set; } = null!;
}
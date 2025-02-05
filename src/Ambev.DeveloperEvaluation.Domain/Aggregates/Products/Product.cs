using Ambev.DeveloperEvaluation.Domain.Aggregates.Products.Events;
using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Exceptions;
using Ambev.DeveloperEvaluation.Domain.ValueObjects;

namespace Ambev.DeveloperEvaluation.Domain.Aggregates.Products;

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

    public string Title { get; private set; } = null!;
    public decimal Price { get; private set; }
    public string Description { get; private set; } = null!;
    public string Category { get; private set; } = null!;
    public string Image { get; private set; } = null!;
    public Rating Rating { get; private set; } = null!;

    public Product ChangePrice(decimal newPrice, TimeProvider timeProvider)
    {
        if (newPrice == Price) return this;

        if (newPrice < 0)
            throw new DomainException("Price cannot be negative");

        AddDomainEvent(ProductPriceChangedEvent.CreateFrom(this, newPrice, timeProvider));
        Price = newPrice;
        return this;
    }
    public Product ChangeTitle(string newTitle)
    {
        if (newTitle == Title) return this;

        if (string.IsNullOrEmpty(newTitle))
            throw new DomainException("Title cannot be null or empty");

        Title = newTitle;
        return this;
    }
    public Product ChangeDescription(string newDescription)
    {
        if (newDescription == Description) return this;

        if (string.IsNullOrEmpty(newDescription))
            throw new DomainException("Description cannot be null or empty");

        Description = newDescription;
        return this;
    }
    public Product ChangeCategory(string newCategory)
    {
        if (newCategory == Category) return this;

        if (string.IsNullOrEmpty(newCategory))
            throw new DomainException("Category cannot be null or empty");

        Category = newCategory;
        return this;
    }
    public Product ChangeImage(string newImage)
    {
        if (newImage == Image) return this;

        if (string.IsNullOrEmpty(newImage))
            throw new DomainException("Image cannot be null or empty");

        Image = newImage;
        return this;
    }
    public Product ChangeRating(Rating newRating)
    {
        if (newRating == Rating) return this;

        Rating = newRating;
        return this;
    }
}
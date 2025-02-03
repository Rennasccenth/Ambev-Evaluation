using Ambev.DeveloperEvaluation.Common.Results;
using Ambev.DeveloperEvaluation.Domain.ValueObjects;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Products.Commands.UpdateProduct;

public sealed class UpdateProductCommand : IRequest<ApplicationResult<UpdateProductCommandResult>>
{
    public UpdateProductCommand(Guid id, string title, decimal price, string description, string category, string image, Rating rating)
    {
        Id = id;
        Title = title;
        Price = price;
        Description = description;
        Category = category;
        Image = image;
        Rating = rating;
    }

    public Guid Id { get; private set; }
    public string Title { get; private set; }
    public decimal Price { get; private set; }
    public string Description { get; private set; }
    public string Category { get; private set; }
    public string Image { get; private set; }
    public Rating Rating { get; private set; }
}

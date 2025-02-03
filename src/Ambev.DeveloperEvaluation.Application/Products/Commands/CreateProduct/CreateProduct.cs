using Ambev.DeveloperEvaluation.Common.Results;
using Ambev.DeveloperEvaluation.Domain.ValueObjects;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Products.Commands.CreateProduct;

public sealed class CreateProductCommand : IRequest<ApplicationResult<CreateProductCommandResult>>
{
    public required string Title { get; set; }
    public required decimal Price { get; set; }
    public required string Description { get; set; }
    public required string Category { get; set; }
    public required string Image { get; set; }
    public required Rating Rating { get; set; }
}

public sealed class CreateProductCommandResult
{
    public required Guid Id { get; init; }
    public required string Title { get; init; }
    public required decimal Price { get; init; }
    public required string Description { get; init; }
    public required string Category { get; init; }
    public required string Image { get; init; }
    public required Rating Rating { get; init; }
}
namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.Commands.DeleteProduct;

public sealed class DeleteProductRequest
{
    public Guid Id { get; init; }

    public DeleteProductRequest(Guid id)
    {
        Id = id;
    }
}
namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.Queries.GetProduct;

public sealed class GetProductRequest
{
    public Guid Id { get; }

    public GetProductRequest(Guid id)
    {
        Id = id;
    }
}
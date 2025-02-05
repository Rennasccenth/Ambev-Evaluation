namespace Ambev.DeveloperEvaluation.Domain.Aggregates.Carts.Services;

public interface ICartsService
{
    Task<Cart> AddCartProductsAsync(Guid userId, Dictionary<Guid, uint> productQuantitiesDictionary, CancellationToken ct);

    Task<Cart> RemoveCartProductsAsync(Guid userId, Dictionary<Guid, uint> productQuantitiesDictionary, CancellationToken ct);
}
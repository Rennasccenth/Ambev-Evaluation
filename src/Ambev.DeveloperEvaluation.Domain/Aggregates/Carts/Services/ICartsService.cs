namespace Ambev.DeveloperEvaluation.Domain.Aggregates.Carts.Services;

public interface ICartsService
{
    /// <summary>
    /// Adds products into the user cart. If the user doens't own one, it was created. If theres overlapping products,
    /// their quantities will be updated
    /// </summary>
    Task<Cart> GetOrCreateUserCart(Guid userId, CancellationToken ct);
    
    Task<Cart> UpsertCartProductsAsync(Guid userId, Dictionary<Guid, uint> productQuantitiesDictionary, CancellationToken ct);

    Task<Cart> RemoveCartProductsAsync(Guid userId, Dictionary<Guid, uint> productQuantitiesDictionary, CancellationToken ct);
    Task RemoveCartAsync(Guid userId, CancellationToken ct);
}
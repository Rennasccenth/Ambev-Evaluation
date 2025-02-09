using Ambev.DeveloperEvaluation.Common.Results;

namespace Ambev.DeveloperEvaluation.Domain.Aggregates.Carts.Services;

public interface ICartsService
{
    Task<ApplicationResult<Cart>> GetCartById(Guid cartId, CancellationToken ct = default);
    Task<ApplicationResult<Cart>> GetCartByUserId(Guid userId, CancellationToken ct = default);
    
    /// <summary>
    /// Creates a Cart bound to a User.
    /// </summary>
    Task<ApplicationResult<Cart>> CreateUserCartAsync(Guid userId, IEnumerable<CartProduct> cartProducts, CancellationToken ct = default);

    /// <summary>
    /// Creates a Cart bound to a User.
    /// </summary>
    Task<ApplicationResult<Cart>> CreateUserCartAsync(Guid userId, CancellationToken ct = default);

    /// <summary>
    /// Creates a Cart without binding it to a User. 
    /// </summary>
    Task<ApplicationResult<Cart>> CreateGenericCart(CancellationToken ct = default);
    
    /// <summary>
    /// Creates a Cart without binding it to a User. 
    /// </summary>
    Task<ApplicationResult<Cart>> CreateGenericCart(IEnumerable<CartProduct> cartProducts, CancellationToken ct = default);

    Task<ApplicationResult<Cart>> UpdateCartAsync(Cart cart, CancellationToken ct = default);
    Task<ApplicationResult<Cart>> RemoveCartProductsAsync(Guid userId, Dictionary<Guid, uint> productQuantitiesDictionary, CancellationToken ct);
    Task<ApplicationResult<Cart>> DeleteCartAsync(Guid cartId, CancellationToken ct = default);
    Task<ApplicationResult<Cart>> DeleteCartByUserIdAsync(Guid userId, CancellationToken ct = default);
}
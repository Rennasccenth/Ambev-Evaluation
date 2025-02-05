using System.Security;
using Ambev.DeveloperEvaluation.Common.Security;
using Ambev.DeveloperEvaluation.Domain.Aggregates.Carts.Exceptions;
using Ambev.DeveloperEvaluation.Domain.Aggregates.Carts.Repositories;
using Ambev.DeveloperEvaluation.Domain.Aggregates.Products.Repositories;

namespace Ambev.DeveloperEvaluation.Domain.Aggregates.Carts.Services;

public sealed class CartsService : ICartsService
{
    private readonly TimeProvider _timeProvider;
    private readonly ICartRepository _cartRepository;
    private readonly IProductRegistryRepository _productRegistryRepository;
    private readonly IUserContext _userContext;

    public CartsService(TimeProvider timeProvider,
        ICartRepository cartRepository,
        IProductRegistryRepository productRegistryRepository,
        IUserContext userContext)
    {
        _timeProvider = timeProvider;
        _cartRepository = cartRepository;
        _productRegistryRepository = productRegistryRepository;
        _userContext = userContext;
    }

    public async Task<Cart> UpsertCartProductsAsync(Guid userId, Dictionary<Guid, uint> productQuantitiesDictionary, CancellationToken ct = default)
    {
        EnsureCartOwnership(userId);

        var allProductsExists = await _productRegistryRepository.ExistsAllAsync(productQuantitiesDictionary.Keys, ct);
        if (!allProductsExists)
        {
            throw new CartValidationException("Not every product in the list exists.");
        }

        Cart userCart = await GetOrCreateUserCart(userId, ct);

        foreach ((Guid productId, var productQuantity) in productQuantitiesDictionary)
        {
            userCart.AddProduct(productId, (int)productQuantity);
        }

        userCart = await _cartRepository.UpsertAsync(userCart, ct);
        return userCart;
    }

    public async Task<Cart> GetOrCreateUserCart(Guid userId, CancellationToken ct)
    {
        EnsureCartOwnership(userId);

        Cart? foundCart = await _cartRepository.FindByUserIdAsync(userId, ct);

        if (foundCart is not null) return foundCart;

        foundCart = await _cartRepository.UpsertAsync(new Cart(userId, _timeProvider.GetUtcNow().DateTime), ct);

        return foundCart;
    }

    public async Task<Cart> RemoveCartProductsAsync(Guid userId, Dictionary<Guid, uint> productQuantitiesDictionary, CancellationToken ct)
    {
        EnsureCartOwnership(userId);

        Cart? userCart = await _cartRepository.FindByUserIdAsync(userId, ct);
        if (userCart is null)
        {
            throw new InexistentCartException($"Cart doesn't exists for userId {userId}");
        }
        foreach ((Guid productId, var productQuantity) in productQuantitiesDictionary)
        {
            userCart.RemoveProduct(productId, (int)productQuantity);
        }
        return userCart;
    }

    public Task RemoveCartAsync(Guid userId, CancellationToken ct)
    {
        EnsureCartOwnership(userId);

        return _cartRepository.DeleteAsync(userId, ct);
    }


    /// <summary>
    /// Verify if the current logged user has the ownership of the cart.
    /// </summary>
    /// <exception cref="SecurityException"></exception>
    private void EnsureCartOwnership(Guid userId)
    {
        if (_userContext.IsAuthenticated && _userContext.UserId != userId)
        {
            throw new SecurityException($"Invalid cart access for userId {userId}. The following UserId {_userContext.UserId} tried to access it.");
        }
    }
}
using Ambev.DeveloperEvaluation.Domain.Aggregates.Carts.Exceptions;
using Ambev.DeveloperEvaluation.Domain.Repositories.Carts;
using Ambev.DeveloperEvaluation.Domain.Repositories.Products;

namespace Ambev.DeveloperEvaluation.Domain.Aggregates.Carts.Services;

public sealed class CartsService : ICartsService
{
    private readonly TimeProvider _timeProvider;
    private readonly ICartRepository _cartRepository;
    private readonly IProductRegistryRepository _productRegistryRepository;

    public CartsService(TimeProvider timeProvider,
        ICartRepository cartRepository,
        IProductRegistryRepository productRegistryRepository)
    {
        _timeProvider = timeProvider;
        _cartRepository = cartRepository;
        _productRegistryRepository = productRegistryRepository;
    }

    public async Task<Cart> AddCartProductsAsync(Guid userId, Dictionary<Guid, uint> productQuantitiesDictionary, CancellationToken ct = default)
    {
        var allProductsExists = await _productRegistryRepository.ExistsAllAsync(productQuantitiesDictionary.Keys, ct);
        if (!allProductsExists)
        {
            throw new CartValidationException("Not every product in the cart exists.");
        }

        Cart? userCart = await _cartRepository.FindByUserIdAsync(userId, ct);
        if (userCart is null)
        {
            userCart = new Cart(userId, _timeProvider.GetUtcNow().DateTime);
            userCart = await _cartRepository.UpsertAsync(userCart, ct);
        }

        foreach ((Guid productId, var productQuantity) in productQuantitiesDictionary)
        {
            userCart.AddProduct(productId, (int)productQuantity);
        }

        return userCart;
    }

    public async Task<Cart> RemoveCartProductsAsync(Guid userId, Dictionary<Guid, uint> productQuantitiesDictionary, CancellationToken ct)
    {
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
}
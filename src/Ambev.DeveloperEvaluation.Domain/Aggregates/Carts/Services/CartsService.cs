using Ambev.DeveloperEvaluation.Domain.Aggregates.Carts.Exceptions;
using Ambev.DeveloperEvaluation.Domain.Aggregates.Carts.Repositories;
using Ambev.DeveloperEvaluation.Domain.Aggregates.Products.Repositories;

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

    public async Task<Cart> UpsertCartProductsAsync(Guid userId, Dictionary<Guid, uint> productQuantitiesDictionary, CancellationToken ct = default)
    {
        var allProductsExists = await _productRegistryRepository.ExistsAllAsync(productQuantitiesDictionary.Keys, ct);
        if (!allProductsExists)
        {
            throw new CartValidationException("Not every product in the list exists.");
        }

        Cart userCart = await _cartRepository.FindByUserIdAsync(userId, ct) 
                         ?? new Cart(userId, _timeProvider.GetUtcNow().DateTime);

        foreach ((Guid productId, var productQuantity) in productQuantitiesDictionary)
        {
            userCart.AddProduct(productId, (int)productQuantity);
        }

        userCart = await _cartRepository.UpsertAsync(userCart, ct);
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
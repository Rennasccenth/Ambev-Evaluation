using System.Security;
using Ambev.DeveloperEvaluation.Common.Errors;
using Ambev.DeveloperEvaluation.Common.Results;
using Ambev.DeveloperEvaluation.Common.Security;
using Ambev.DeveloperEvaluation.Domain.Aggregates.Carts.Exceptions;
using Ambev.DeveloperEvaluation.Domain.Aggregates.Carts.Repositories;
using Ambev.DeveloperEvaluation.Domain.Aggregates.Products.Repositories;
using IUserContext = Ambev.DeveloperEvaluation.Domain.Abstractions.IUserContext;

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

    public async Task<ApplicationResult<Cart>> GetCartById(Guid cartId, CancellationToken ct = default)
    {
        Cart? foundCart = await _cartRepository.FindByCartIdAsync(cartId, ct);
        if (foundCart is null)
        {
            return ApplicationError.NotFoundError($"Cart ID {cartId} wasn't found.");
        }
        
        return foundCart;
    }

    public async Task<ApplicationResult<Cart>> GetCartByUserId(Guid userId, CancellationToken ct = default)
    {
        if (_userContext.UserId != userId)
        {
            return ApplicationError.UnauthorizedAccessError("User doesn't have permission to access this resource.");            
        }

        Cart? foundCart = await _cartRepository.FindByUserIdAsync(userId, ct);
        if (foundCart is null)
        {
            return ApplicationError.NotFoundError($"Cart from User ID {userId} wasn't found.");
        }

        return foundCart;
    }

    public async Task<ApplicationResult<Cart>> UpdateCartAsync(Cart cart, CancellationToken ct = default)
    {
        var updatingProductsExists = await  _productRegistryRepository
            .ExistsAllAsync(cart.Products.Select(prod => prod.ProductId), ct);
        if (!updatingProductsExists)
        {
            return ApplicationError.UnprocessableError("One or more products in the updating cart doesn't exists in the product registry.");
        }

        var cartWasUpdated = await _cartRepository.UpdateAsync(cart, ct);
        if (!cartWasUpdated)
        {
            return ApplicationError.UnprocessableError("Cart wasn't updated.");
        }

        return cart;
    }

    public async Task<ApplicationResult<Cart>> DeleteCartAsync(Guid cartId, CancellationToken ct = default)
    {
        Cart? userCart = await _cartRepository.FindByCartIdAsync(cartId, ct);
        if (userCart is null)
        {
            return ApplicationError.NotFoundError($"Cart ID {cartId} wasn't found.");
        }

        await _cartRepository.DeleteAsync(userCart.Id, ct);

        return userCart;
    }

    public async Task<ApplicationResult<Cart>> DeleteCartByUserIdAsync(Guid userId, CancellationToken ct = default)
    {
        ApplicationError? ownershipError = EnsureResourceOwnership(userId);
        if (ownershipError is not null) return ownershipError;

        Cart? userCart = await _cartRepository.FindByUserIdAsync(userId, ct);
        if (userCart is null)
        {
            return ApplicationError.NotFoundError($"Cart from User ID {userId} wasn't found.");
        }

        await _cartRepository.DeleteAsync(userCart.Id, ct);

        return userCart;
    }

    public async Task<ApplicationResult<Cart>> CreateUserCartAsync(Guid userId, IEnumerable<CartProduct> cartProducts, CancellationToken ct = default)
    {
        try
        {
            ApplicationError? ownershipError = EnsureResourceOwnership(userId);
            if (ownershipError is not null) return ownershipError;

            List<CartProduct> enumeratedCartProducts = cartProducts.ToList();
            var allProductsExists = await _productRegistryRepository.ExistsAllAsync(enumeratedCartProducts.Select(cp => cp.ProductId), ct);
            if (allProductsExists is false)
            {
                return ApplicationError.NotFoundError("One or more products in the creating cart doesn't exists in the system.");
            }
            
            Cart? userExistentCart = await _cartRepository.FindByUserIdAsync(userId, ct);
            if (userExistentCart is not null)
            {
                return ApplicationError.DuplicatedResourceError("User already has a Cart."); 
            }

            Cart userCart = await _cartRepository.CreateAsync(new Cart(userId, _timeProvider.GetUtcNow().DateTime, enumeratedCartProducts), ct);
            
            return userCart;
        }
        catch (DuplicatedCartException duplicatedCartException)
        {
            return ApplicationError.DuplicatedResourceError(duplicatedCartException.Message);
        }    
    }

    public async Task<ApplicationResult<Cart>> CreateUserCartAsync(Guid userId, CancellationToken ct = default)
    {
        try
        {
            ApplicationError? ownershipError = EnsureResourceOwnership(userId);
            if (ownershipError is not null) return ownershipError;

            Cart? userExistentCart = await _cartRepository.FindByUserIdAsync(userId, ct);
            if (userExistentCart is not null)
            {
                return ApplicationError.DuplicatedResourceError("User already has a Cart."); 
            }

            Cart userCart = await _cartRepository.CreateAsync(new Cart(userId, _timeProvider.GetUtcNow().DateTime), ct);
            return userCart;
        }
        catch (DuplicatedCartException duplicatedCartException)
        {
            return ApplicationError.DuplicatedResourceError(duplicatedCartException.Message);
        }
    }

    public async Task<ApplicationResult<Cart>> CreateGenericCart(CancellationToken ct = default)
    {
        try
        {
            Cart userCart = await _cartRepository.CreateAsync(new Cart(_timeProvider.GetUtcNow().DateTime), ct);
            return userCart;
        }
        catch (DuplicatedCartException duplicatedCartException)
        {
            return ApplicationError.DuplicatedResourceError(duplicatedCartException.Message);
        }
    }

    public async Task<ApplicationResult<Cart>> CreateGenericCart(IEnumerable<CartProduct> cartProducts, CancellationToken ct = default)
    {
        try
        {
            List<CartProduct> enumeratedCarts = cartProducts.ToList();
            var allProductsExists = await _productRegistryRepository.ExistsAllAsync(enumeratedCarts.Select(cp => cp.ProductId), ct);
            if (allProductsExists is false)
            {
                return ApplicationError.NotFoundError("One or more products in the creating cart doesn't exists in the system.");
            }
            
            Cart userCart = await _cartRepository.CreateAsync(new Cart(_timeProvider.GetUtcNow().DateTime, enumeratedCarts), ct);
            return userCart;
        }
        catch (DuplicatedCartException duplicatedCartException)
        {
            return ApplicationError.DuplicatedResourceError(duplicatedCartException.Message);
        }
    }

    /// <summary>
    /// Verify if the current logged user has the ownership of the cart.
    /// </summary>
    /// <exception cref="SecurityException"></exception>
    private ApplicationError? EnsureResourceOwnership(Guid userId)
    {
        if (_userContext.IsAuthenticated && _userContext.UserId != userId)
        {
            return ApplicationError.UnauthorizedAccessError(
                $"Invalid cart access for userId {userId}. The following UserId {_userContext.UserId} tried to access it.");
        }
        
        return null;
    }
}
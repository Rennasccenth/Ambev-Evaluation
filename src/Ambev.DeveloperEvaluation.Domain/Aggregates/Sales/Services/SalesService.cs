using Ambev.DeveloperEvaluation.Common.Errors;
using Ambev.DeveloperEvaluation.Common.Results;
using Ambev.DeveloperEvaluation.Domain.Aggregates.Carts;
using Ambev.DeveloperEvaluation.Domain.Aggregates.Carts.Repositories;
using Ambev.DeveloperEvaluation.Domain.Aggregates.Carts.Strategies;
using Ambev.DeveloperEvaluation.Domain.Aggregates.Sales.Repositories;
using Ambev.DeveloperEvaluation.Domain.Aggregates.Users.Enums;
using Ambev.DeveloperEvaluation.Domain.Events;
using Ambev.DeveloperEvaluation.Domain.Specifications;
using Microsoft.Extensions.Logging;
using IUserContext = Ambev.DeveloperEvaluation.Domain.Abstractions.IUserContext;

namespace Ambev.DeveloperEvaluation.Domain.Aggregates.Sales.Services;

public sealed class SalesService : ISalesService
{
    private readonly TimeProvider _timeProvider;
    private readonly ISaleRepository _saleRepository;
    private readonly IDiscountStrategy _discountStrategy;
    private readonly IDomainEventDispatcher _domainEventDispatcher;
    private readonly ICartRepository _cartRepository;
    private readonly IUserContext _userContext;
    private readonly ISalesCounter _salesCounter;
    private readonly ILogger<SalesService> _logger;

    public SalesService(
        TimeProvider timeProvider,
        ISaleRepository saleRepository,
        IDiscountStrategy discountStrategy,
        IDomainEventDispatcher domainEventDispatcher,
        ISalesCounter salesCounter,
        ILogger<SalesService> logger,
        ICartRepository cartRepository,
        IUserContext userContext)
    {
        _timeProvider = timeProvider;
        _saleRepository = saleRepository;
        _discountStrategy = discountStrategy;
        _domainEventDispatcher = domainEventDispatcher;
        _salesCounter = salesCounter;
        _logger = logger;
        _cartRepository = cartRepository;
        _userContext = userContext;
    }

    public async Task<ApplicationResult<Sale>> CreateSaleAsync(
        Cart cart,
        string branch,
        IProductPriceResolver productPriceResolver,
        ISpecification<Cart>? specification = null,
        CancellationToken ct = default)
    {
        _logger.LogInformation("Creating sale for customer {CustomerId}", cart.UserId);
        
        var productsUnitPrices = await productPriceResolver
            .ResolveProductsUnitPriceAsync(cart.Products.Select(p => p.ProductId), ct);

        if (specification is not null && !specification.IsSatisfiedBy(cart))
        {
            return ApplicationError.UnprocessableError("Cart item limit was exceeded.");
        }

        if (_userContext.IsAuthenticated is false)
        {
            return ApplicationError.UnprocessableError("Cannot create a sale for a non logged user.");
        }

        if(_userContext.UserId != cart.UserId)
        {
            return ApplicationError.UnprocessableError("User must be the owner of the cart used to create the sale.");
        }

        if (cart.Products.Count == 0)
        {
            return ApplicationError.UnprocessableError("Cannot create a sale with no products.");
        }

        if (cart.UserId is null)
        {
            return ApplicationError.UnprocessableError("The cart must have a user associated with.");
        }

        long salesCounter = await _salesCounter.GetNextSaleNumberAsync(ct);
        Sale creatingSale = Sale.Create(cart.UserId.Value, salesCounter, branch, _timeProvider);

        // Convert all products in the cart to sale products.
        foreach ((Guid productId, var unitPrice) in productsUnitPrices)
        {
            // Calculates per product discounts
            int productCount = cart.CountProducts(p => p.ProductId == productId);
            decimal discount = _discountStrategy.GetDiscountPercentage(productCount);
            decimal discountedPrice = unitPrice * (1 - discount);

            creatingSale.AddProduct(new SaleProduct(creatingSale.Id, productId, discountedPrice, productCount, discount), _timeProvider);
        }

        Sale createdSale = await _saleRepository.CreateAsync(creatingSale, ct);
        await _cartRepository.DeleteAsync(cart.Id, ct);
        await _domainEventDispatcher.DispatchAndClearEventsAsync(createdSale);

        _logger.LogInformation("Sale created for customer {CustomerId}", cart.UserId);
        return createdSale;
    }

    public async Task<ApplicationResult<Sale>> CancelSaleAsync(Guid saleId, CancellationToken ct)
    {
        _logger.LogInformation("Cancelling sale {SaleId}", saleId);
        Sale? sale = await _saleRepository.FindByIdAsync(saleId, ct);
        if (sale is null)
        {
            return ApplicationError.NotFoundError($"SaleId {saleId} wasn't found.");
        }

        if (_userContext.UserRole is UserRole.Customer && _userContext.UserId != sale.CustomerId)
        {
            return ApplicationError.UnprocessableError("Customer must be the owner of the sale.");
        }

        if (sale.Canceled)
        {
            return ApplicationError.UnprocessableError("Cannot cancel a already canceled sale.");
        }
        if (sale.Terminated)
        {
            return ApplicationError.UnprocessableError("Cannot cancel a concluded sale.");
        }

        Sale canceledSale = sale.Cancel(_timeProvider);
        await _saleRepository.UpdateAsync(canceledSale, ct);
        await _domainEventDispatcher.DispatchAndClearEventsAsync(canceledSale);
        
        _logger.LogInformation("Sale {SaleId} cancelled", saleId);
        return canceledSale;
    }

    public async Task<ApplicationResult<Sale>> ConcludeSaleAsync(Guid saleId, CancellationToken ct)
    {
        _logger.LogInformation("Concluding sale {SaleId}", saleId);
        
        Sale? sale = await _saleRepository.FindByIdAsync(saleId, ct);
        if (sale is null)
        {
            return ApplicationError.NotFoundError($"SaleId {saleId} wasn't found.");
        }
        
        if (_userContext.UserRole is UserRole.Customer)
        {
            return ApplicationError.PermissionDeniedError("A Customer user cannot conclude a Sale.");
        }

        if (sale.Canceled)
        {
            return ApplicationError.UnprocessableError("Cannot conclude a concluded sale.");
        }
        if (sale.Terminated)
        {
            return ApplicationError.UnprocessableError("Cannot conclude a already concluded sale.");
        }

        Sale concludedSale = sale.Sell(_timeProvider);
        await _saleRepository.UpdateAsync(concludedSale, ct);
        await _domainEventDispatcher.DispatchAndClearEventsAsync(concludedSale);
        
        _logger.LogInformation("Sale {SaleId} concluded", saleId);
        return concludedSale;
    }
}
using Ambev.DeveloperEvaluation.Domain.Aggregates.Carts;
using Ambev.DeveloperEvaluation.Domain.Aggregates.Carts.Exceptions;
using Ambev.DeveloperEvaluation.Domain.Aggregates.Carts.Strategies;
using Ambev.DeveloperEvaluation.Domain.Aggregates.Sales.Exceptions;
using Ambev.DeveloperEvaluation.Domain.Aggregates.Sales.Repositories;
using Ambev.DeveloperEvaluation.Domain.Events;
using Ambev.DeveloperEvaluation.Domain.Specifications;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Domain.Aggregates.Sales.Services;

public sealed class SalesService : ISalesService
{
    private readonly TimeProvider _timeProvider;
    private readonly ISaleRepository _saleRepository;
    private readonly IDiscountStrategy _discountStrategy;
    private readonly IDomainEventDispatcher _domainEventDispatcher;
    private readonly ILogger<SalesService> _logger;

    public SalesService(
        TimeProvider timeProvider,
        ISaleRepository saleRepository,
        IDiscountStrategy discountStrategy,
        IDomainEventDispatcher domainEventDispatcher,
        ILogger<SalesService> logger)
    {
        _timeProvider = timeProvider;
        _saleRepository = saleRepository;
        _discountStrategy = discountStrategy;
        _domainEventDispatcher = domainEventDispatcher;
        _logger = logger;
    }

    public async Task<Sale> CreateSaleAsync(
        Cart cart,
        string branch,
        IProductPriceResolver productPriceResolver,
        ISpecification<Cart> specification,
        CancellationToken ct = default)
    {
        _logger.LogInformation("Creating sale for customer {CustomerId}", cart.CustomerId);
        
        var productsUnitPrices = await productPriceResolver.ResolveProductsUnitPriceAsync(cart.Products.Select(p => p.ProductId), ct);
        List<SaleProduct> sellingProducts = [];

        if (!specification.IsSatisfiedBy(cart))
        {
            throw new CartValidationException("Cart is invalid.");
        }

        Sale creatingSale = Sale.Create(cart.CustomerId, branch, _timeProvider);
        // Convert all products in the cart to sale products.
        foreach ((Guid productId, var unitPrice) in productsUnitPrices)
        {
            // Calculates per product discounts
            int productCount = cart.CountProducts(p => p.ProductId == productId);
            decimal discount = _discountStrategy.GetDiscountPercentage(productCount);
            decimal discountedPrice = unitPrice * (1 - discount);

            creatingSale.AddProduct(new SaleProduct(creatingSale.Id, productId, discountedPrice, productCount), _timeProvider);
        }

        Sale createdSale = await _saleRepository.CreateAsync(creatingSale, ct);

        // Dispatch any events
        await _domainEventDispatcher.DispatchAndClearEventsAsync(createdSale);

        _logger.LogInformation("Sale created for customer {CustomerId}", cart.CustomerId);
        return createdSale;
    }

    public async Task<Sale?> CancelSaleAsync(Guid saleId, CancellationToken ct)
    {
        _logger.LogInformation("Cancelling sale {SaleId}", saleId);
        var sale = await _saleRepository.FindByIdAsync(saleId, ct);
        if (sale is null)
        {
            throw new SaleNotFoundException($"SaleId {saleId} wasn't found.");
        }

        var canceledSale = sale.Cancel(_timeProvider);
        await _domainEventDispatcher.DispatchAndClearEventsAsync(canceledSale);
        
        _logger.LogInformation("Sale {SaleId} cancelled", saleId);
        return canceledSale;
    }

    public async Task<Sale?> ConcludeSaleAsync(Guid saleId, CancellationToken ct)
    {
        _logger.LogInformation("Concluding sale {SaleId}", saleId);
        
        var sale = await _saleRepository.FindByIdAsync(saleId, ct);
        if (sale is null)
        {
            throw new SaleNotFoundException($"SaleId {saleId} wasn't found.");
        }
        
        var concludedSale = sale.Cancel(_timeProvider);
        await _domainEventDispatcher.DispatchAndClearEventsAsync(concludedSale);
        
        _logger.LogInformation("Sale {SaleId} concluded", saleId);
        return concludedSale;
    }
}
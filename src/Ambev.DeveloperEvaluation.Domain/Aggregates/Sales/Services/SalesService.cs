using Ambev.DeveloperEvaluation.Domain.Aggregates.Carts;
using Ambev.DeveloperEvaluation.Domain.Aggregates.Carts.Exceptions;
using Ambev.DeveloperEvaluation.Domain.Aggregates.Carts.Strategies;
using Ambev.DeveloperEvaluation.Domain.Aggregates.Sales.Exceptions;
using Ambev.DeveloperEvaluation.Domain.Events.Abstractions;
using Ambev.DeveloperEvaluation.Domain.Repositories.Sales;
using Ambev.DeveloperEvaluation.Domain.Specifications;

namespace Ambev.DeveloperEvaluation.Domain.Aggregates.Sales.Services;

public sealed class SalesService : ISalesService
{
    private readonly TimeProvider _timeProvider;
    private readonly ISaleRepository _saleRepository;
    private readonly IDiscountStrategy _discountStrategy;
    private readonly IDomainEventDispatcher _domainEventDispatcher;

    public SalesService(
        TimeProvider timeProvider,
        ISaleRepository saleRepository,
        IDiscountStrategy discountStrategy,
        IDomainEventDispatcher domainEventDispatcher)
    {
        _timeProvider = timeProvider;
        _saleRepository = saleRepository;
        _discountStrategy = discountStrategy;
        _domainEventDispatcher = domainEventDispatcher;
    }

    public async Task<Sale> CreateSaleAsync(
        Cart cart,
        string branch,
        IProductPriceResolver productPriceResolver,
        ISpecification<Cart> specification,
        CancellationToken ct = default)
    {
        var productsUnitPrices = await productPriceResolver.ResolveProductsUnitPriceAsync(cart.Products.Select(p => p.ProductId), ct);
        List<SaleProduct> sellingProducts = [];

        if (specification.IsSatisfiedBy(cart))
        {
            throw new CartValidationException("Cart is invalid.");
        }
        
        foreach ((Guid productId, var unitPrice) in productsUnitPrices)
        {
            int productCount = cart.CountProducts(p => p.ProductId == productId);

            decimal discount = _discountStrategy.GetDiscountPercentage(productCount);
            decimal discountedPrice = unitPrice * (1 - discount);

            sellingProducts.Add(new SaleProduct(productId, discountedPrice, productCount));
        }

        var creatingSale = Sale.Create(cart.CustomerId, sellingProducts, branch, _timeProvider);
        Sale createdSale = await _saleRepository.CreateAsync(creatingSale, ct);

        await _domainEventDispatcher.DispatchAndClearEventsAsync(createdSale);
        return createdSale;
    }

    public async Task<Sale?> CancelSaleAsync(Guid saleId, CancellationToken ct)
    {
        var sale = await _saleRepository.FindByIdAsync(saleId, ct);
        if (sale is null)
        {
            throw new SaleNotFoundException($"SaleId {saleId} wasn't found.");
        }

        var canceledSale = sale.Cancel(_timeProvider);
        await _domainEventDispatcher.DispatchAndClearEventsAsync(canceledSale);
        return canceledSale;
    }

    public async Task<Sale?> ConcludeSaleAsync(Guid saleId, CancellationToken ct)
    {
        var sale = await _saleRepository.FindByIdAsync(saleId, ct);
        if (sale is null)
        {
            throw new SaleNotFoundException($"SaleId {saleId} wasn't found.");
        }
        
        var concludedSale = sale.Cancel(_timeProvider);
        await _domainEventDispatcher.DispatchAndClearEventsAsync(concludedSale);
        return concludedSale;
    }
}
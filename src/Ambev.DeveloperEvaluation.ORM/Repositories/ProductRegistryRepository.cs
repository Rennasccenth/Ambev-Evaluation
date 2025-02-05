using System.Linq.Expressions;
using Ambev.DeveloperEvaluation.Application.Products.Exceptions;
using Ambev.DeveloperEvaluation.Common.Results;
using Ambev.DeveloperEvaluation.Domain.Aggregates.Products;
using Ambev.DeveloperEvaluation.Domain.Aggregates.Products.Repositories;
using Ambev.DeveloperEvaluation.Domain.Common;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace Ambev.DeveloperEvaluation.ORM.Repositories;

internal sealed class ProductRegistryRepository : IProductRegistryRepository
{
    private readonly DefaultContext _dbContext;
    private const string DescendingSortOrderIndicator = "desc";
    public ProductRegistryRepository(DefaultContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Product?> FindByIdAsync(Guid id, CancellationToken ct)
    {
        return await _dbContext.Products.FirstAsync(product => product.Id == id, cancellationToken: ct);
    }

    public async Task<PaginatedList<Product>> GetByFilterAsync(GetRegisteredProductsQueryFilter queryFilter, CancellationToken ct)
    {
        IQueryable<Product> query = _dbContext.Set<Product>()
            .AsNoTracking()
            .AsQueryable();

        query = ApplyProductsFiltering(query, queryFilter);

        if (!string.IsNullOrWhiteSpace(queryFilter.OrderBy))
        {
            query = ApplySortExpression(query, queryFilter.OrderBy);
        }

        return await PaginatedList<Product>.CreateAsync(query, queryFilter.CurrentPage, queryFilter.PageSize, ct);
    }

    public async ValueTask<bool> ExistsAllAsync(IEnumerable<Guid> productIds, CancellationToken ct)
    {
        return await _dbContext.Products
            .AsNoTracking()
            .AllAsync(x => productIds.Contains(x.Id), cancellationToken: ct);
    }

    private static readonly Dictionary<string, Expression<Func<Product, object>>> SortingKeyMap = new()
    {
        ["id"] =  product => product.Id,
        ["title"] = product => product.Title,
        ["price"] = product => product.Price,
        ["description"] = product => product.Description,
        ["category"] = product => product.Category,
        ["rate"] = product => product.Rating.Rate,
        ["count"] = product => product.Rating.Count
    };

    private static IOrderedQueryable<Product> ApplySortExpression(IQueryable<Product> filteredItems, string orderByString)
    {
        var orderingCandidates = orderByString.Trim().Split(',');

        IOrderedQueryable<Product> orderingItems = filteredItems.OrderBy(p => 0);
        
        foreach (var orderCandidate in orderingCandidates)
        {
            var orderingPair = orderCandidate.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            var orderingKey = orderingPair.FirstOrDefault();

            if (string.IsNullOrEmpty(orderingKey)) continue;

            // Key selector didn't match known ordering expressions
            if (!SortingKeyMap.TryGetValue(orderingKey, out var orderingExpression)) continue;

            if (orderingPair.Length == 2)
            {
                var orderingSelector = orderingPair[1];
                orderingItems = orderingSelector == DescendingSortOrderIndicator
                    ? orderingItems.ThenByDescending(orderingExpression)
                    : orderingItems.ThenBy(orderingExpression);
            }
            else
            {
                orderingItems = orderingItems.ThenBy(orderingExpression);
            }
        }

        return orderingItems;
    }

    private static IQueryable<Product> ApplyProductsFiltering(IQueryable<Product> products, GetRegisteredProductsQueryFilter queryFilter)
    {
        return products.ApplyFilters(queryFilter.FilterBy);
    }

    public async Task<Product> CreateAsync(Product creatingProduct, CancellationToken ct)
    {
        var addedEntry = await _dbContext.Products.AddAsync(creatingProduct, ct);
        try
        {
            await _dbContext.SaveChangesAsync(ct);
        } catch (DbUpdateException ex) when (ex.InnerException is PostgresException { SqlState: "23505" })
        {
            throw new DuplicatedProductException();
        }

        return addedEntry.Entity;
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken ct)
    {
        var deletedRows = await _dbContext.Products.Where(product => product.Id == id).ExecuteDeleteAsync(ct);
        return deletedRows is 1;
    }

    public async Task<Product> UpdateAsync(Product updatingProduct, CancellationToken ct)
    {
        var updatingEntry = _dbContext.Products.Update(updatingProduct);
        try
        {
            await _dbContext.SaveChangesAsync(ct);
        }catch (DbUpdateException ex) when (ex.InnerException is PostgresException { SqlState: "23505" })
        {
            throw new DuplicatedProductException();
        }
        return updatingEntry.Entity;
    }

    public Task<List<string>> GetCategoriesAsync(CancellationToken ct)
    {
        return _dbContext.Products.AsNoTracking()
            .Select(product => product.Category)
            .Distinct()
            .ToListAsync(ct);
    }
}
using Ambev.DeveloperEvaluation.Application.Products.Commands.UpdateProduct;
using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories.Products;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace Ambev.DeveloperEvaluation.ORM.Repositories;

internal sealed class ProductRepository : IProductRepository
{
    private readonly DefaultContext _dbContext;

    public ProductRepository(DefaultContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Product?> FindByIdAsync(Guid id, CancellationToken ct)
    {
        return await _dbContext.Products.FirstAsync(product => product.Id == id, cancellationToken: ct);
    }

    public Task<PaginatedList<Product>> GetByFilter(GetProductsQueryFilter queryFilter, CancellationToken ct)
    {
        throw new NotImplementedException();
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
}
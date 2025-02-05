namespace Ambev.DeveloperEvaluation.Domain.Aggregates.Sales.Services;

public interface ISalesCounter
{
    ValueTask<long> GetNextSaleNumberAsync(CancellationToken cancellationToken = default);
}
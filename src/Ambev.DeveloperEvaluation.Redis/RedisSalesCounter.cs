using Ambev.DeveloperEvaluation.Domain.Aggregates.Sales.Services;
using StackExchange.Redis;

namespace Ambev.DeveloperEvaluation.Redis;

public sealed class RedisSalesCounter : ISalesCounter
{
    private readonly IDatabase _database;
    private const string SaleCounterKey = "sale-counter";
    
    public RedisSalesCounter(IDatabase database)
    {
        _database = database;
    }

    public async ValueTask<long> GetNextSaleNumberAsync(CancellationToken cancellationToken = default)
    {
        return await _database.StringIncrementAsync(SaleCounterKey);
    }
}
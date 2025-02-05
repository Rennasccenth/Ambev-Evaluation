using System.Globalization;
using Ambev.DeveloperEvaluation.Domain.Aggregates.Sales;
using Ambev.DeveloperEvaluation.Domain.Aggregates.Sales.Repositories;
using Ambev.DeveloperEvaluation.Domain.Common;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Ambev.DeveloperEvaluation.MongoDB.Repositories;

public sealed class SaleRepository : ISaleRepository
{
    private readonly IMongoCollection<Sale> _saleCollection;

    public SaleRepository(IMongoCollection<Sale> saleCollection)
    {
        _saleCollection = saleCollection;
    }

    public async Task<Sale?> FindByIdAsync(Guid id, CancellationToken ct)
    {
        var filter = Builders<Sale>.Filter.Eq(s => s.Id, id);
        return await _saleCollection.Find(filter).FirstOrDefaultAsync(ct);
    }

    public async Task<PaginatedList<Sale>> GetByFilterAsync(GetSalesQueryFilter queryFilter, CancellationToken ct)
    {
        var filter = BuildFilter(queryFilter);
        var totalItems = await _saleCollection.CountDocumentsAsync(filter, cancellationToken: ct);
        
        var query = _saleCollection.Find(filter)
            .Sort(Builders<Sale>.Sort.Descending(s => s.CreatedDate))
            .Skip((queryFilter.CurrentPage - 1) * queryFilter.PageSize)
            .Limit(queryFilter.PageSize);

        var results = await query.ToListAsync(ct);
        
        return PaginatedList<Sale>.FromConsolidatedList(results, queryFilter.CurrentPage, queryFilter.PageSize, totalItems);
    }

    public async Task<Sale> CreateAsync(Sale creatingSale, CancellationToken ct)
    {
        await _saleCollection.InsertOneAsync(creatingSale, cancellationToken: ct);
        return creatingSale;
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken ct)
    {
        var filter = Builders<Sale>.Filter.Eq(s => s.Id, id);
        var result = await _saleCollection.DeleteOneAsync(filter, ct);
        return result.DeletedCount == 1;
    }

    public async Task<Sale> UpdateAsync(Sale updatingSale, CancellationToken ct)
    {
        var filter = Builders<Sale>.Filter.Eq(s => s.Id, updatingSale.Id);
        await _saleCollection.ReplaceOneAsync(filter, updatingSale, cancellationToken: ct);
        return updatingSale;
    }

    private static FilterDefinition<Sale> BuildFilter(GetSalesQueryFilter filter)
    {
        var builder = Builders<Sale>.Filter;
        var filterDefinitions = new List<FilterDefinition<Sale>>();
        
        var fieldMappings = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            ["customer"] = nameof(Sale.CustomerId),
            ["branch"] = nameof(Sale.Branch),
            ["createdDate"] = nameof(Sale.CreatedDate),
            ["canceled"] = nameof(Sale.CanceledDate),
            ["product"] = "Products.ProductId",
            ["productPrice"] = "Products.UnitPrice"
        };

        foreach (var (key, value) in filter.FilterBy)
        {
            if (string.IsNullOrWhiteSpace(value)) continue;

            // Handle range filters (_min/_max)
            if (TryHandleRangeFilter(key, value, fieldMappings, builder, out var rangeFilter))
            {
                filterDefinitions.Add(rangeFilter);
                continue;
            }

            // Handle special field mappings
            var fieldName = key.ToLower() switch
            {
                "customer" => nameof(Sale.CustomerId),
                "branch" => nameof(Sale.Branch),
                "date" => nameof(Sale.CreatedDate),
                "canceled" => nameof(Sale.CanceledDate),
                "product" => "Products.ProductId",
                _ => key
            };

            // Handle wildcard filters
            if (value.Contains('*'))
            {
                var regexPattern = value.Replace("*", ".*");
                filterDefinitions.Add(builder.Regex(fieldName, new BsonRegularExpression(regexPattern, "i")));
                continue;
            }

            // Handle numeric/date values
            if (TryParseNumericValue(value, out var numericValue))
            {
                filterDefinitions.Add(builder.Eq(fieldName, numericValue));
            }
            else if (DateTime.TryParse(value, out var dateValue))
            {
                filterDefinitions.Add(builder.Eq(fieldName, dateValue));
            }
            else
            {
                filterDefinitions.Add(builder.Eq(fieldName, value));
            }
        }

        return filterDefinitions.Count > 0 
            ? builder.And(filterDefinitions) 
            : builder.Empty;
    }

    private static bool TryHandleRangeFilter(string key, string value, 
        Dictionary<string, string> fieldMappings,
        FilterDefinitionBuilder<Sale> builder,
        out FilterDefinition<Sale> filter)
    {
        filter = null;
        const string minPrefix = "_min";
        const string maxPrefix = "_max";

        try
        {
            if (key.StartsWith(minPrefix))
            {
                var field = key.Substring(minPrefix.Length);
                if (!fieldMappings.TryGetValue(field, out var mappedField)) return false;
                
                filter = DateTime.TryParse(value, out var dateValue)
                    ? builder.Gte(mappedField, dateValue)
                    : builder.Gte(mappedField, double.Parse(value));
                
                return true;
            }

            if (key.StartsWith(maxPrefix))
            {
                var field = key.Substring(maxPrefix.Length);
                if (!fieldMappings.TryGetValue(field, out var mappedField)) return false;
                
                filter = DateTime.TryParse(value, out var dateValue)
                    ? builder.Lte(mappedField, dateValue)
                    : builder.Lte(mappedField, double.Parse(value));
                
                return true;
            }
        }
        catch (FormatException)
        {
            return false;
        }

        return false;
    }

    private static bool TryParseNumericValue(string value, out double numericValue)
    {
        return double.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out numericValue);
    }
}
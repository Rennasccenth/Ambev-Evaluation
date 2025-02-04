using Ambev.DeveloperEvaluation.WebApi.Common;
using Microsoft.AspNetCore.Mvc;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.Queries.GetProducts;

public sealed class GetProductsRequest : PaginatedRequest
{
    [FromQuery(Name = "category")] public string? Category { get; set; }
    
    [FromQuery(Name = "title")] public string? Title { get; set; }

    [FromQuery(Name = "description")] public string? Description { get; set; }

    [FromQuery(Name = "_minPrice")] public int? MinPrice { get; set; }
    [FromQuery(Name = "price")] public int? Price { get; set; }
    [FromQuery(Name = "_maxPrice")] public int? MaxPrice { get; set; }

    [FromQuery(Name = "_minRate")] public int? MinRate { get; set; }
    [FromQuery(Name = "rate")] public int? Rate { get; set; }
    [FromQuery(Name = "_maxRate")] public int? MaxRate { get; set; }

    [FromQuery(Name = "_minCount")] public int? MinCount { get; set; }
    [FromQuery(Name = "count")] public int? Count { get; set; }
    [FromQuery(Name = "_maxCount")] public int? MaxCount { get; set; }
}
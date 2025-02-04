using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Primitives;

namespace Ambev.DeveloperEvaluation.WebApi.Common;

public abstract class PaginatedRequest
{
    private const int DefaultPageSize = 15;
    private const string PageSizeFieldName = "_size";
    private const string CurrentPageFieldName = "_page";
    private const string OrderByFieldName = "_order";

    [FromQuery(Name = CurrentPageFieldName)] public int CurrentPage { get; set; } = 1;
    [FromQuery(Name = PageSizeFieldName)] public int PageSize { get; set; } = DefaultPageSize;
    [FromQuery(Name = OrderByFieldName)] public string? OrderBy { get; init; }
    [JsonIgnore, BindNever] public Dictionary<string, string?> FilterBy { get; set; } = [];

    [JsonIgnore]
    private static readonly HashSet<string> DontMapThosePropertiesHashSet =
    [
        PageSizeFieldName,
        CurrentPageFieldName,
        OrderByFieldName
    ];

    public void SetFilter(IQueryCollection queryCollection)
    {
        foreach ((var key, StringValues values) in queryCollection)
        {
            // O(1) lookup
            if (DontMapThosePropertiesHashSet.Contains(key)) continue;
            FilterBy.TryAdd(key, values.FirstOrDefault());
        }
    }
}
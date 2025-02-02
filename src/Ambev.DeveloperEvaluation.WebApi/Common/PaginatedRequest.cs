using Microsoft.AspNetCore.Mvc;

namespace Ambev.DeveloperEvaluation.WebApi.Common;

public abstract class PaginatedRequest
{
    private const int DefaultPageSize = 15;

    [FromQuery(Name = "_page")] public int CurrentPage { get; set; } = 1;
    [FromQuery(Name = "_size")] public int PageSize { get; set; } = DefaultPageSize;
    [FromQuery(Name = "_order")] public string? OrderBy { get; set; } = null;
}
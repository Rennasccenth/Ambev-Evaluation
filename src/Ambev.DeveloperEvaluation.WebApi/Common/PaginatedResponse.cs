using Ambev.DeveloperEvaluation.Domain.Common;

namespace Ambev.DeveloperEvaluation.WebApi.Common;

public class PaginatedResponse<T>
{
    public List<T> Data { get; init; } = [];
    public long TotalCount { get; init; }
    public int PageNumber { get; init; }
    public int PageSize { get; init; }
    public int TotalPages { get; init; }

    public static PaginatedResponse<T> FromPaginatedList(PaginatedList<T> paginatedList)
    {
        return new PaginatedResponse<T>
        {
            Data = paginatedList,
            TotalCount = paginatedList.TotalItems,
            PageNumber = paginatedList.CurrentPage,
            PageSize = paginatedList.PageSize,
            TotalPages = paginatedList.TotalPages
        };
    }
}
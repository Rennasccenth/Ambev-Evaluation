using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.Domain.Common;

public class PaginatedList<T> : List<T>
{
    public int CurrentPage { get; }
    public int TotalPages { get; }
    public int PageSize { get; }
    public long TotalItems { get; }

    public bool HasPrevious => CurrentPage > 1;
    public bool HasNext => CurrentPage < TotalPages;

    private PaginatedList(List<T> items, long totalItems, int currentPage, int pageSize)
    {
        TotalItems = totalItems;
        PageSize = pageSize;
        CurrentPage = currentPage;
        TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

        AddRange(items);
    }

    public static async Task<PaginatedList<T>> CreateAsync(IQueryable<T> source, int pageNumber, int pageSize, CancellationToken cancellationToken = default)
    {
        var totalItems = await source
            .CountAsync(cancellationToken: cancellationToken);

        var items = await source
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken: cancellationToken);

        return new PaginatedList<T>(items, totalItems, pageNumber, pageSize);
    }
    
    public static PaginatedList<T> FromConsolidatedList(List<T> items,
        int currentPage,
        int pageSize,
        long totalItems)
    {
        return new PaginatedList<T>(items, totalItems, currentPage, pageSize);
    }
}
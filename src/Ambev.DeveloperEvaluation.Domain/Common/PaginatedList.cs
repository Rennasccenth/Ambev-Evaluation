using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.Domain.Common;

public class PaginatedList<T> : List<T>
{
    public int CurrentPage { get; private set; }
    public int TotalPages { get; private set; }
    public int PageSize { get; private set; }
    public int TotalItems { get; private set; }

    public bool HasPrevious => CurrentPage > 1;
    public bool HasNext => CurrentPage < TotalPages;

    private PaginatedList(List<T> items, int totalItems, int currentPage, int pageSize)
    {
        TotalItems = totalItems;
        PageSize = pageSize;
        CurrentPage = currentPage;
        TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

        AddRange(items);
    }

    public static async Task<PaginatedList<T>> CreateAsync(IQueryable<T> source, int pageNumber, int pageSize, CancellationToken cancellationToken = default)
    {
        var count = await source
            .CountAsync(cancellationToken: cancellationToken);

        var items = await source
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken: cancellationToken);

        return new PaginatedList<T>(items, count, pageNumber, pageSize);
    }
}
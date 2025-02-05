namespace Ambev.DeveloperEvaluation.Domain.Repositories.User;

public sealed class GetUsersQueryFilter
{
    public int CurrentPage { get; init; } = 1;
    public int PageSize { get; init; } = 10;
    public string? OrderBy { get; init; }
    public required IReadOnlyDictionary<string, string> FilterBy { get; init; } = new Dictionary<string, string>();
}
namespace Ambev.DeveloperEvaluation.WebApi.Features.Users.Queries.GetUsers;

public sealed class GetUsersQueryResponse
{
    public required List<GetUsersQueryItem> Data { get; init; }
    public required int TotalCount { get; init; }
    public required int Page { get; init; }
    public required int PageSize { get; init; }
    public required int TotalPages { get; init; }
}
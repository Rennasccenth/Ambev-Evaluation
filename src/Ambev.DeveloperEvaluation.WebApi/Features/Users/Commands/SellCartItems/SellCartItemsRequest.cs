namespace Ambev.DeveloperEvaluation.WebApi.Features.Users.Commands.SellCartItems;

public sealed class SellCartItemsRequest
{
    public Guid UserId { get; }
    public string BranchName { get; }

    public SellCartItemsRequest(Guid userId, string branchName)
    {
        UserId = userId;
        BranchName = branchName;
    }
}

public sealed class SellCartItemsRequestBody
{
    public required string BranchName { get; init; }
}
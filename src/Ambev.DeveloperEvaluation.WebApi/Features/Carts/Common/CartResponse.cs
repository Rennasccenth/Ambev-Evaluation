namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts.Common;

public sealed class CartResponse
{
    public required string Id { get; init; }
    public required string UserId { get; init; }
    public required string Date { get; init; }
    public required List<ProductQuantifier> Products { get; init; }
}
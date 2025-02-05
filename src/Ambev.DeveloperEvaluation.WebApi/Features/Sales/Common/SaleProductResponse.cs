namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.Common;

public sealed class SaleProductResponse
{
    public Guid Id { get; init; }
    public int Quantity { get; init; }
    public decimal UnitPrice { get; init; }
    public decimal TotalPrice { get; init; }
}
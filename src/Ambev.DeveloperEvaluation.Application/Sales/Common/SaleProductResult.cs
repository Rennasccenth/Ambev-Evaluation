namespace Ambev.DeveloperEvaluation.Application.Sales.Common;

public class SaleProductResult
{
    public Guid ProductId { get; init; }
    public Guid SaleId { get; init; }
    public int Quantity { get; init; }
    public decimal UnitPrice { get; init; }
    public decimal TotalPrice  { get; init; }
}
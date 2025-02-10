namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.Commands.ConcludeSale;

public sealed class ConcludeSaleRequest
{
    public ConcludeSaleRequest(Guid saleId)
    {
        SaleId = saleId;
    }

    public Guid SaleId { get; init; }
}
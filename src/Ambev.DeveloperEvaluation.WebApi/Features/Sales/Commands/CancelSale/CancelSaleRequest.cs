namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.Commands.CancelSale;

public sealed class CancelSaleRequest
{
    public CancelSaleRequest(Guid saleId)
    {
        SaleId = saleId;
    }

    public Guid SaleId { get; init; }
}
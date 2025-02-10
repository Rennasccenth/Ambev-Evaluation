namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.Queries;

public sealed class GetSaleRequest
{
    public GetSaleRequest(Guid saleId)
    {
        SaleId = saleId;
    }

    public Guid SaleId { get; set; }
}
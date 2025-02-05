namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.Common;

public sealed class SaleResponse
{
    public Guid Id { get; set; }
    public Guid CustomerId { get; init; }
    public long Number { get; set; }
    public string Branch { get; set; } = null!;
    public List<SaleProductResponse> Products { get; init; }
    public DateTime? CreatedDate { get; set; }
    public DateTime? TerminationDate { get; set; }
    public DateTime? CanceledDate { get; set; }
    public bool Canceled => CanceledDate.HasValue;
    public bool Terminated => TerminationDate.HasValue;
    public decimal TotalAmount => Products.Sum(prod => prod.TotalPrice);
}
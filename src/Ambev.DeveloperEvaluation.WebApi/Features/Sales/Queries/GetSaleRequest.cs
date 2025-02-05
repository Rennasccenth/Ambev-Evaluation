using Ambev.DeveloperEvaluation.Application.Sales.Common;
using Ambev.DeveloperEvaluation.Application.Sales.Queries;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.Queries;

public sealed class GetSaleRequest
{
    public GetSaleRequest(Guid saleId)
    {
        SaleId = saleId;
    }

    public Guid SaleId { get; set; }
}

public sealed class GetSaleProfile : Profile
{
    public GetSaleProfile()
    {
        // In
        CreateMap<GetSaleRequest, GetSaleQuery>();

        
    }
}


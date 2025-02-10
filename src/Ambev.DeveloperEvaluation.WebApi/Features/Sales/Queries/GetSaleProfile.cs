using Ambev.DeveloperEvaluation.Application.Sales.Queries;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.Queries;

public sealed class GetSaleProfile : Profile
{
    public GetSaleProfile()
    {
        // In
        CreateMap<GetSaleRequest, GetSaleQuery>();
    }
}
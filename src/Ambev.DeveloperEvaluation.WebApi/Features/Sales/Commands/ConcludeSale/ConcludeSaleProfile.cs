using Ambev.DeveloperEvaluation.Application.Sales.Commands.ConcludeSale;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.Commands.ConcludeSale;

public sealed class ConcludeSaleProfile : Profile
{
    public ConcludeSaleProfile()
    {
        CreateMap<ConcludeSaleRequest, ConcludeSaleCommand>();
    }
}
using System.Globalization;
using Ambev.DeveloperEvaluation.Application.Products.Queries.GetProduct;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.Queries.GetProduct;

public sealed class GetProductProfile : Profile
{
    public GetProductProfile()
    {
        CreateMap<GetProductRequest, GetProductQuery>();
        CreateMap<GetProductQueryResult, GetProductResponse>()
            .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price.ToString(CultureInfo.InvariantCulture)));
    }
}
using Ambev.DeveloperEvaluation.Application.Carts.Common;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts.Common;

public class ProductQuantifierProfile : Profile
{
    public ProductQuantifierProfile()
    {
        CreateMap<ProductQuantifier, ProductSummary>()
            .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductId))
            .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity));
    }
}
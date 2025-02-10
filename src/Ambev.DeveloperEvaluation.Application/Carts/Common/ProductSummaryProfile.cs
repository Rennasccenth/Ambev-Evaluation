using Ambev.DeveloperEvaluation.Domain.Aggregates.Carts;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Carts.Common;

public sealed class ProductSummaryProfile : Profile
{
    public ProductSummaryProfile()
    {
        CreateMap<CartProduct, ProductSummary>()
            .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductId))
            .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity))
            .ReverseMap();
    }
}
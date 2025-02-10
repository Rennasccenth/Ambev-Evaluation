using Ambev.DeveloperEvaluation.Domain.Aggregates.Carts;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Carts.Common;

public sealed class CartProfile : Profile
{
    public CartProfile()
    {
        // Out
        CreateMap<Cart, CartResult>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
            .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.Date))
            .ForMember(dest => dest.Products, opt => opt.MapFrom(src => src.Products));
    }
}
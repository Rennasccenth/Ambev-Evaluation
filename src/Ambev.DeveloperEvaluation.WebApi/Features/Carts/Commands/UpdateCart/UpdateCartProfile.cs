using Ambev.DeveloperEvaluation.Application.Carts.Commands.UpdateCart;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts.Commands.UpdateCart;

public sealed class UpdateCartProfile : Profile
{
    public UpdateCartProfile()
    {
        // In
        CreateMap<UpdateCartRequest, UpdateCartCommand>()
            .ForMember(dest => dest.CartId, opt => opt.MapFrom(src => src.CartId))
            .ForMember(dest => dest.Products, opt => opt.MapFrom(src => src.Products));
    }
}
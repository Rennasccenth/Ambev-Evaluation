using Ambev.DeveloperEvaluation.Application.Carts.Commands;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts.Commands.CreateCart;

public sealed class CreateCartProfile : Profile
{
    public CreateCartProfile()
    {
        // In
        CreateMap<CreateCartRequest, CreateCartCommand>()
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
            .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.Date))
            .ForMember(dest => dest.Products, opt => opt.MapFrom(src => src.Products));
    }
}
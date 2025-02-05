using Ambev.DeveloperEvaluation.Application.Carts.Commands;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts.Commands.UpsertCart;

public sealed class UpsertCartProfile : Profile
{
    public UpsertCartProfile()
    {
        // In
        CreateMap<UpsertCartRequest, UpsertCartCommand>()
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
            .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.Date))
            .ForMember(dest => dest.Products, opt => opt.MapFrom(src => src.Products));
    }
}
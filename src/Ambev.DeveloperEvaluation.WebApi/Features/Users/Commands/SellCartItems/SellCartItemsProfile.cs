using Ambev.DeveloperEvaluation.Application.Users.Commands.SellCartItems;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Users.Commands.SellCartItems;

public sealed class SellCartItemsProfile : Profile
{
    public SellCartItemsProfile()
    {
        // In
        CreateMap<SellCartItemsRequest, SellCartItemsCommand>()
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId));
    }
}
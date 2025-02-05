using Ambev.DeveloperEvaluation.Application.Carts.Queries;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts.Queries.GetCart;

public sealed class GetCartProfile : Profile
{
    public GetCartProfile()
    {
        // In
        CreateMap<GetCartRequest, GetCartQuery>()
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId));
    }
}
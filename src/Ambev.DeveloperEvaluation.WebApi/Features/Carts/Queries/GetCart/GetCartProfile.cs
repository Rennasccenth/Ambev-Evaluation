using Ambev.DeveloperEvaluation.Application.Carts.Queries;
using Ambev.DeveloperEvaluation.Application.Carts.Queries.GetCart;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts.Queries.GetCart;

public sealed class GetCartProfile : Profile
{
    public GetCartProfile()
    {
        // In
        CreateMap<GetCartRequest, GetCartQuery>()
            .ForMember(dest => dest.CartId, opt => opt.MapFrom(src => src.CartId));
    }
}
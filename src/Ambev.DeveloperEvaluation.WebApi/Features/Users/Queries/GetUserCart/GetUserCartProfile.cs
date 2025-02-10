using Ambev.DeveloperEvaluation.Application.Users.Queries.GetUserCart;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Users.Queries.GetUserCart;

public sealed class GetUserCartProfile : Profile
{
    public GetUserCartProfile()
    {
        CreateMap<GetUserCartRequest, GetUserCartQuery>()
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId));
    }
}
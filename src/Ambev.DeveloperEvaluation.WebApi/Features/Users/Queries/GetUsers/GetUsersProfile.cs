using Ambev.DeveloperEvaluation.Application.Users.Queries.GetUsers;
using Ambev.DeveloperEvaluation.Domain.Enums;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Users.Queries.GetUsers;

public sealed class GetUsersProfile : Profile
{
    public GetUsersProfile()
    {
        // In
        CreateMap<GetUsersRequest, GetUsersQuery>()
            .ForMember(dest => dest.CurrentPage, opt => opt.MapFrom(src => src.CurrentPage))
            .ForMember(dest => dest.PageSize, opt => opt.MapFrom(src => src.PageSize))
            .ForMember(dest => dest.OrderBy, opt => opt.MapFrom(src => src.OrderBy))
            .ForMember(dest => dest.FilterBy, opt => opt.MapFrom(src => src.FilterBy));

        // Out
        CreateMap<GetUsersQueryResult, GetUsersResponse>()
            .ForMember(dest => dest.Data, opt => opt.MapFrom(src => src.Users))
            .ForMember(dest => dest.PageNumber, opt => opt.MapFrom(src => src.Page))
            .ForMember(dest => dest.PageSize, opt => opt.MapFrom(src => src.PageSize))
            .ForMember(dest => dest.TotalCount, opt => opt.MapFrom(src => src.TotalCount))
            .ForMember(dest => dest.TotalPages, opt => opt.MapFrom(src => src.TotalPages));

        CreateMap<GetUsersQuerySummary, GetUsersRequestItem>();
    }
}
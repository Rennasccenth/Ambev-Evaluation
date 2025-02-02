using Ambev.DeveloperEvaluation.Application.Users.Queries.GetUsers;
using Ambev.DeveloperEvaluation.Domain.Enums;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Users.Queries.GetUsers;

public sealed class GetUsersProfile : Profile
{
    public GetUsersProfile()
    {
        CreateMap<GetUsersRequest, GetUsersQuery>()
            .ForMember(dest => dest.Page, opt => opt.MapFrom(src => src.CurrentPage))
            .ForMember(dest => dest.PageSize, opt => opt.MapFrom(src => src.PageSize))
            .ForMember(dest => dest.OrderBy, opt => opt.MapFrom(src => src.OrderBy))
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Username))
            .ForMember(dest => dest.Firstname, opt => opt.MapFrom(src => src.Firstname))
            .ForMember(dest => dest.Lastname, opt => opt.MapFrom(src => src.Lastname))
            .ForMember(dest => dest.Street, opt => opt.MapFrom(src => src.Street))
            .ForMember(dest => dest.Number, opt => opt.MapFrom(src => src.Number))
            .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.City))
            .ForMember(dest => dest.Lat, opt => opt.MapFrom(src => src.Lat))
            .ForMember(dest => dest.Long, opt => opt.MapFrom(src => src.Long))
            .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.Phone))
            .ForMember(dest => dest.Status,
                opt => opt.PreCondition(src => src.Status != null && Enum.IsDefined(typeof(UserStatus), src.Status)))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => Enum.Parse<UserStatus>(src.Status!, true)))
            .ForMember(dest => dest.Role,
                opt => opt.PreCondition(src => src.Role != null && Enum.IsDefined(typeof(UserRole), src.Role)))
            .ForMember(dest => dest.Role, opt => opt.MapFrom(src => Enum.Parse<UserRole>(src.Role!, true)));

        CreateMap<GetUsersQueryResult, GetUsersQueryResponse>()
            .ForMember(dest => dest.Data, opt => opt.MapFrom(src => src.Users));
        

        CreateMap<GetUsersQuerySummary, GetUsersQueryItem>();
    }
}
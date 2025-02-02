using System.ComponentModel;
using Ambev.DeveloperEvaluation.Application.Users.Queries.GetUser;
using Ambev.DeveloperEvaluation.Domain.Enums;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Users.Queries.GetUser;

public sealed class GetUserProfile : Profile
{
    public GetUserProfile()
    {
        CreateMap<Guid, GetUserCommand>()
            .ConstructUsing(id => new GetUserCommand(id));

        CreateMap<GetUserResult, GetUserResponse>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(usr => usr.Id))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(usr => usr.Email))
            .ForMember(dest => dest.Username, opt => opt.MapFrom(usr => usr.Username))
            .ForMember(dest => dest.Password, opt => opt.MapFrom(usr => usr.Password))
            .ForPath(dest => dest.Name.Firstname, opt => opt.MapFrom(usr => usr.Name.Firstname))
            .ForPath(dest => dest.Name.Lastname, opt => opt.MapFrom(usr => usr.Name.Lastname))
            .ForPath(dest => dest.Address.City, opt => opt.MapFrom(usr => usr.Address.City))
            .ForPath(dest => dest.Address.Street, opt => opt.MapFrom(usr => usr.Address.Street))
            .ForPath(dest => dest.Address.Number, opt => opt.MapFrom(usr => usr.Address.Number))
            .ForPath(dest => dest.Address.Zipcode, opt => opt.MapFrom(usr => usr.Address.Zipcode))
            .ForPath(dest => dest.Address.Geolocation.Lat, opt => opt.MapFrom(usr => usr.Address.Geolocation.Latitude))
            .ForPath(dest => dest.Address.Geolocation.Long,
                opt => opt.MapFrom(usr => usr.Address.Geolocation.Longitude))
            .ForMember(dest => dest.Phone, opt => opt.MapFrom(usr => usr.Phone))
            .ForMember(dest => dest.Status,
                opt => opt.MapFrom(usr => new EnumConverter(typeof(UserStatus)).ConvertToString(usr.Status)))
            .ForMember(dest => dest.Role,
                opt => opt.MapFrom(usr => new EnumConverter(typeof(UserRole)).ConvertToString(usr.Role)));
    }
}

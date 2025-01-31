using AutoMapper;
using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Application.Users.GetUser;

public sealed class GetUserProfile : Profile
{
    public GetUserProfile()
    {
        CreateMap<User, GetUserResult>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(usr => usr.Id))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(usr => usr.Email))
            .ForMember(dest => dest.Username, opt => opt.MapFrom(usr => usr.Username))
            .ForMember(dest => dest.Password, opt => opt.MapFrom(usr => usr.Password))
            .ForPath(dest => dest.Name.Firstname, opt => opt.MapFrom(usr => usr.Firstname))
            .ForPath(dest => dest.Name.Lastname, opt => opt.MapFrom(usr => usr.Lastname))
            .ForPath(dest => dest.Address.City, opt => opt.MapFrom(usr => usr.Address.City))
            .ForPath(dest => dest.Address.Street, opt => opt.MapFrom(usr => usr.Address.Street))
            .ForPath(dest => dest.Address.Number, opt => opt.MapFrom(usr => usr.Address.Number))
            .ForPath(dest => dest.Address.Zipcode, opt => opt.MapFrom(usr => usr.Address.ZipCode))
            .ForPath(dest => dest.Address.Geolocation.Latitude, opt => opt.MapFrom(usr => usr.Address.Latitude))
            .ForPath(dest => dest.Address.Geolocation.Longitude, opt => opt.MapFrom(usr => usr.Address.Longitude))
            .ForMember(dest => dest.Phone, opt => opt.MapFrom(usr => usr.Phone))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(usr => usr.Status))
            .ForMember(dest => dest.Role, opt => opt.MapFrom(usr => usr.Role));    }
}
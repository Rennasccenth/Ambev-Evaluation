using Ambev.DeveloperEvaluation.Domain.Entities;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Users.Commands.CreateUser;

public sealed class CreateUserProfile : Profile
{
    public CreateUserProfile()
    {
        CreateMap<User, CreateUserResult>()
            .ForMember(result => result.Id, opt => opt.MapFrom(usr => usr.Id))
            .ForMember(result => result.Email, opt => opt.MapFrom(usr => usr.Email))
            .ForMember(result => result.Username, opt => opt.MapFrom(usr => usr.Username))
            .ForMember(result => result.Password, opt => opt.MapFrom(usr => usr.Password))
            .ForPath(result => result.Name.Firstname, opt => opt.MapFrom(usr => usr.Firstname))
            .ForPath(result => result.Name.Lastname, opt => opt.MapFrom(usr => usr.Lastname))
            .ForPath(result => result.Address.City, opt => opt.MapFrom(usr => usr.Address.City))
            .ForPath(result => result.Address.Street, opt => opt.MapFrom(usr => usr.Address.Street))
            .ForPath(result => result.Address.Number, opt => opt.MapFrom(usr => usr.Address.Number))
            .ForPath(result => result.Address.Zipcode, opt => opt.MapFrom(usr => usr.Address.ZipCode))
            .ForPath(result => result.Address.Geolocation.Latitude, opt => opt.MapFrom(usr => usr.Address.Latitude))
            .ForPath(result => result.Address.Geolocation.Longitude, opt => opt.MapFrom(usr => usr.Address.Longitude))
            .ForMember(result => result.Phone, opt => opt.MapFrom(usr => usr.Phone))
            .ForMember(result => result.Status, opt => opt.MapFrom(usr => usr.Status))
            .ForMember(result => result.Role, opt => opt.MapFrom(usr => usr.Role));
    }
}

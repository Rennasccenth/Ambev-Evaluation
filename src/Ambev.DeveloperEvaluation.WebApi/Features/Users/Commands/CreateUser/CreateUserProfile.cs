using System.ComponentModel;
using Ambev.DeveloperEvaluation.Application.Users.Commands.CreateUser;
using Ambev.DeveloperEvaluation.Domain.Aggregates.Users.Enums;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Users.Commands.CreateUser;

public sealed class CreateUserProfile : Profile
{
    public CreateUserProfile()
    {
        CreateMap<CreateUserRequest, CreateUserCommand>()
            .ForMember(command => command.Firstname, member => member.MapFrom(req => req.Name.Firstname))
            .ForMember(command => command.Lastname, member => member.MapFrom(req => req.Name.Lastname))
            .ForMember(command => command.City, member => member.MapFrom(req => req.Address.City))
            .ForMember(command => command.Street, member => member.MapFrom(req => req.Address.Street))
            .ForMember(command => command.Number, member => member.MapFrom(req => req.Address.Number))
            .ForMember(command => command.ZipCode, member => member.MapFrom(req => req.Address.Zipcode))
            .ForMember(command => command.Latitude, member => member.MapFrom(req => req.Address.Geolocation.Lat))
            .ForMember(command => command.Longitude, member => member.MapFrom(req => req.Address.Geolocation.Long))
            .ForMember(command => command.Status, member => member.MapFrom(req => new EnumConverter(typeof(UserStatus)).ConvertFromString(req.Status)))
            .ForMember(command => command.Role, member => member.MapFrom(req => new EnumConverter(typeof(UserRole)).ConvertFromString(req.Role)));

        CreateMap<CreateUserResult, CreateUserResponse>()
            .ForMember(response => response.Id, member => member.MapFrom(result => result.Id))
            .ForPath(response => response.Name.Firstname, member => member.MapFrom(result => result.Name.Firstname))
            .ForPath(response => response.Name.Lastname, member => member.MapFrom(result => result.Name.Lastname))
            .ForPath(response => response.Address.City, expression => expression.MapFrom(result => result.Address.City))
            .ForPath(response => response.Address.Street, expression => expression.MapFrom(result => result.Address.Street))
            .ForPath(response => response.Address.Zipcode, expression => expression.MapFrom(result => result.Address.Zipcode))
            .ForPath(response => response.Address.Number, expression => expression.MapFrom(result => result.Address.Number))
            .ForPath(response => response.Address.Geolocation.Lat, expression => expression.MapFrom(result => result.Address.Geolocation.Latitude))
            .ForPath(response => response.Address.Geolocation.Long, expression => expression.MapFrom(result => result.Address.Geolocation.Longitude))
            .ForMember(response => response.Status, expression => expression.MapFrom(result => new EnumConverter(typeof(UserStatus)).ConvertToString(result.Status)))
            .ForMember(response => response.Role, expression => expression.MapFrom(result => new EnumConverter(typeof(UserRole)).ConvertToString(result.Role)));
    }
}

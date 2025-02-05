using System.ComponentModel;
using Ambev.DeveloperEvaluation.Application.Users.Commands.UpdateUser;
using Ambev.DeveloperEvaluation.Domain.Aggregates.Users.Enums;
using Ambev.DeveloperEvaluation.Domain.ValueObjects;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Users.Commands.UpdateUser;

public sealed class UpdateUserProfile : Profile
{
    public UpdateUserProfile()
    {
        CreateMap<UpdateUserRequest, UpdateUserCommand>()
            .ForMember(dest => dest.Id, expression => expression.MapFrom(src => src.Id))
            .ForMember(dest => dest.Username, expression => expression.MapFrom(src => src.Username))
            .ForMember(dest => dest.Password, expression => expression.MapFrom(src => src.Password))
            .ForMember(dest => dest.Phone, expression => expression.MapFrom(src => src.Phone))
            .ForMember(dest => dest.Email, expression => expression.MapFrom(src => src.Email))
            .ForMember(dest => dest.Address, expression => expression.MapFrom(src => new Address(
                src.Address.City, 
                src.Address.Street, 
                src.Address.Number, 
                src.Address.Zipcode, 
                src.Address.Geolocation.Lat,
                src.Address.Geolocation.Long)))
            .ForMember(dest => dest.Status,
                expression => expression.MapFrom(src => new EnumConverter(typeof(UserStatus)).ConvertFromString(src.Status)))
            .ForMember(dest => dest.Role,
                expression => expression.MapFrom(src => new EnumConverter(typeof(UserRole)).ConvertFromString(src.Role)));
     
        CreateMap<UpdateUserCommandResult, UpdateUserResponse>()
            .ForMember(dest => dest.Id, expression => expression.MapFrom(src => src.Id))
            .ForMember(dest => dest.Username, expression => expression.MapFrom(src => src.Username))
            .ForMember(dest => dest.Password, expression => expression.MapFrom(src => src.Password.ToString()))
            .ForMember(dest => dest.Phone, expression => expression.MapFrom(src => src.Phone.ToString()))
            .ForMember(dest => dest.Name, expression => expression.MapFrom(src => new NameDto
            {
                Firstname = src.Name.Firstname,
                Lastname = src.Name.Lastname
            }))
            .ForMember(dest => dest.Address, expression => expression.MapFrom(src => new UpdatingAddress
            {
                City = src.Address.City,
                Street = src.Address.Street,
                Number = src.Address.Number,
                Zipcode = src.Address.ZipCode,
                Geolocation = new NewGeolocation
                {
                    Lat = src.Address.Latitude,
                    Long = src.Address.Longitude
                }
            }))
            .ForMember(dest => dest.Status, expression => expression.MapFrom(src => new EnumConverter(typeof(UserStatus)).ConvertToString(src.Status)))
            .ForMember(dest => dest.Role, expression => expression.MapFrom(src => new EnumConverter(typeof(UserRole)).ConvertToString(src.Role)));
    }
}
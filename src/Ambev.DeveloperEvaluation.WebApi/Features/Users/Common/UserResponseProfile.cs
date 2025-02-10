using Ambev.DeveloperEvaluation.Application.Users.Common;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Users.Common;

public sealed class UserResponseProfile : Profile
{
    public UserResponseProfile()
    {
        // Out
        CreateMap<UserResult, UserResponse>()
            .ForMember(dest => dest.Id, expression => expression.MapFrom(src => src.Id))
            .ForMember(dest => dest.Email, expression => expression.MapFrom(src => src.Email))
            .ForMember(dest => dest.Username, expression => expression.MapFrom(src => src.Username))
            .ForMember(dest => dest.Password, expression => expression.MapFrom(src => src.Password))
            .ForMember(dest => dest.Phone, expression => expression.MapFrom(src => src.Phone))
            .ForMember(dest => dest.Status, expression => expression.MapFrom(src => src.Status))
            .ForMember(dest => dest.Role, expression => expression.MapFrom(src => src.Role))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => new NameResponse
            {
                Firstname = src.Name.Firstname,
                Lastname = src.Name.Lastname
            }))
            .ForMember(dest => dest.Address, opt => opt.MapFrom(src => new AddressResponse
            {
                City = src.Address.City,
                Street = src.Address.Street,
                Number = src.Address.Number,
                Zipcode = src.Address.Zipcode,
                Geolocation = new GeolocationResponse
                {
                    Lat = src.Address.Geolocation.Lat,
                    Long = src.Address.Geolocation.Long
                }
            }));
    }
}
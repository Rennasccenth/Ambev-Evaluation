using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Users.Common;

public sealed class UserResultProfile : Profile
{
    public UserResultProfile()
    {
        // Out
        CreateMap<Domain.Aggregates.Users.User, UserResult>()
            .ForMember(dest => dest.Id, expression => expression.MapFrom(src => src.Id))
            .ForMember(dest => dest.Email, expression => expression.MapFrom(src => src.Email))
            .ForMember(dest => dest.Username, expression => expression.MapFrom(src => src.Username))
            .ForMember(dest => dest.Password, expression => expression.MapFrom(src => src.Password))
            .ForMember(dest => dest.Phone, expression => expression.MapFrom(src => src.Phone))
            .ForMember(dest => dest.Status, expression => expression.MapFrom(src => src.Status))
            .ForMember(dest => dest.Role, expression => expression.MapFrom(src => src.Role))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => new NameResult
            {
                Firstname = src.Firstname,
                Lastname = src.Lastname
            }))
            .ForMember(dest => dest.Address, opt => opt.MapFrom(src => new AddressResult
            {
                City = src.Address.City,
                Street = src.Address.Street,
                Number = src.Address.Number,
                Zipcode = src.Address.ZipCode,
                Geolocation = new GeolocationResult
                {
                    Lat = src.Address.Latitude,
                    Long = src.Address.Longitude
                }
            }));
    }
}
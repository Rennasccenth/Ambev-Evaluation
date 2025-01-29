using AutoMapper;
using Ambev.DeveloperEvaluation.Application.Users.CreateUser;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Users.CreateUser;

/// <summary>
/// Profile for mapping between Application and API CreateUser responses
/// </summary>
public sealed class CreateUserProfile : Profile
{
    /// <summary>
    /// Initializes the mappings for CreateUser feature
    /// </summary>
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
            .ForMember(command => command.Longitude, member => member.MapFrom(req => req.Address.Geolocation.Long));
    
        CreateMap<CreateUserResult, CreateUserResponse>();
    }
}

using System.ComponentModel;
using Ambev.DeveloperEvaluation.Application.Users.Commands.UpdateUser;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.ValueObjects;
using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Users.Commands.UpdateUser;


public sealed class UpdateUserRequestBody
{
    [FromBody] public required string Username { get; init; }
    [FromBody] public required string Password { get; init; }
    [FromBody] public required string Phone { get; init; }
    [FromBody] public required string Email { get; init; }
    [FromBody] public required UpdatingAddress Address { get; init; }
    [FromBody] public required string Status { get; init; }
    [FromBody] public required string Role { get; init; }
}

public sealed class UpdateUserRequest
{
    public UpdateUserRequest(Guid id, UpdateUserRequestBody requestBody)
    {
        Id = id;
        Username = requestBody.Username;
        Password = requestBody.Password;
        Phone = requestBody.Phone;
        Email = requestBody.Email;
        Address = requestBody.Address;
        Status = requestBody.Status;
        Role = requestBody.Role;
    }
    public Guid Id { get; }
    public string Username { get; }
    public string Password { get; }
    public string Phone { get; }
    public string Email { get; }
    public UpdatingAddress Address { get; }
    public string Status { get; }
    public string Role { get; }
}

public sealed record UpdatingAddress
{
    public required string City { get; init; }
    public required string Street { get; init; }
    public required int Number { get; init; }
    public required string Zipcode { get; init; }
    public required NewGeolocation Geolocation { get; init; }
}

public sealed record NewGeolocation
{
    public required string Lat { get; init; }
    public required string Long { get; init; }
}

public sealed record NameDto
{
    public required string Firstname { get; init; }
    public required string Lastname { get; init; }
}

public sealed record UpdateUserResponse
{
    public required Guid Id { get; init; }
    public required string Email { get; init; }
    public required string Username { get; init; }
    public required string Password { get; init; }
    public required NameDto Name { get; init; }
    public required UpdatingAddress Address { get; init; }
    public required string Phone { get; init; }
    public required string Status { get; init; }
    public required string Role { get; init; }
}

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

public sealed class UpdateUserValidator : AbstractValidator<UpdateUserRequest>
{
    public UpdateUserValidator(
        IValidator<Email> emailValidator,
        IValidator<Phone> phoneValidator,
        IValidator<Password> passwordValidator)
    {
        RuleFor(user => (Email)user.Email)
            .SetValidator(emailValidator);
        RuleFor(user =>(Password)user.Password)
            .SetValidator(passwordValidator);
        RuleFor(user => (Phone)user.Phone)
            .SetValidator(phoneValidator);
        
        RuleFor(user => user.Username)
            .NotNull()
            .NotEmpty().Length(3, 50);
        
        RuleFor(user => user.Address.City)
            .NotNull()
            .NotEmpty()
            .Length(3, 50);
        RuleFor(user => user.Address.Street)
            .NotNull()
            .NotEmpty()
            .Length(3, 70);
        
        RuleFor(user => user.Address.Number)
            .NotNull()
            .NotEmpty()
            .GreaterThan(-1);
        
        RuleFor(user => user.Address.Zipcode)
            .NotNull()
            .NotEmpty()
            .Length(3, 50);
        RuleFor(user => user.Address.Geolocation.Lat)
            .NotNull()
            .NotEmpty()
            .Length(3, 50);
        RuleFor(user => user.Address.Geolocation.Long)
            .NotNull()
            .NotEmpty()
            .Length(3, 50);
        
        RuleFor(user => user.Role).Must(stringRole => Enum.IsDefined(typeof(UserRole), stringRole));
        RuleFor(user => user.Status).Must(stringStatus => Enum.IsDefined(typeof(UserStatus), stringStatus));
    }
}
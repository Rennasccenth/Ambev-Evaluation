using Ambev.DeveloperEvaluation.Application.Users.Queries.GetUsers;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.ValueObjects;
using Ambev.DeveloperEvaluation.WebApi.Common;
using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Users.GetUsers;

public sealed class GetUsersRequest : PaginatedRequest
{
    [FromQuery] public Guid? Id { get; set; }
    [FromQuery] public string? Email { get; set; }
    [FromQuery] public string? Username { get; set; }
    [FromQuery] public string? Firstname { get; set; }
    [FromQuery] public string? Lastname { get; set; }
    [FromQuery] public string? Street { get; set; }
    [FromQuery] public int? Number { get; set; }
    [FromQuery] public string? City { get; set; }
    [FromQuery] public string? Lat { get; set; }
    [FromQuery] public string? Long { get; set; }
    [FromQuery] public string? Phone { get; set; }
    [FromQuery] public string? Status { get; set; }
    [FromQuery] public string? Role { get; set; }
}

public sealed class GetUsersQueryItem
{
    public string Id { get; set; }
    public string Email { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public string Firstname { get; set; }
    public string Lastname { get; set; }
    public Address Address { get; set; }
    public string Phone { get; set; }
    public string Status { get; set; }
    public string Role { get; set; }
}

public sealed class GetUsersQueryResponse
{
    public required List<GetUsersQueryItem> Data { get; init; }
    public required int TotalCount { get; init; }
    public required int Page { get; init; }
    public required int PageSize { get; init; }
    public required int TotalPages { get; init; }
}

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

public sealed class GetUsersValidator : AbstractValidator<GetUsersRequest>
{
    public GetUsersValidator()
    {
        RuleFor(req => req.Status)
            .IsEnumName(typeof(UserStatus), caseSensitive: false).When(req => !string.IsNullOrEmpty(req.Status));
        
        RuleFor(req => req.Role)
            .IsEnumName(typeof(UserRole), caseSensitive: false).When(req => !string.IsNullOrEmpty(req.Role));
    }
}
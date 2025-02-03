using Ambev.DeveloperEvaluation.Common.Results;
using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.Repositories.User;
using Ambev.DeveloperEvaluation.Domain.ValueObjects;
using AutoMapper;
using FluentValidation;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Users.Queries.GetUsers;

public sealed class GetUsersQuery : PaginatedQuery, IRequest<ApplicationResult<GetUsersQueryResult>>
{
    public Guid? Id { get; init; }
    public Email? Email { get; init; }
    public string? Username { get; init; }
    public string? Firstname { get; init; }
    public string? Lastname { get; init; }
    public string? Street { get; init; }
    public string? Zipcode { get; init; }
    public int? Number { get; init; }
    public string? City { get; init; }
    public string? Lat { get; init; }
    public string? Long { get; init; }
    public Phone? Phone { get; init; }
    public UserStatus? Status { get; init; }
    public UserRole? Role { get; init; }
}

public sealed class GetUsersQueryResult
{
    public required List<GetUsersQuerySummary> Users { get; init; }
    public required int TotalCount { get; init; }
    public required int Page { get; init; }
    public required int PageSize { get; init; }
    public required int TotalPages { get; init; }
}

public sealed class GetUsersQuerySummary
{
    public required Guid Id { get; init; }
    public required Email Email { get; init; }
    public required string Username { get; init; }
    public required Password Password { get; init; }
    public required string Firstname { get; init; }
    public required string Lastname { get; init; }
    public required Address Address { get; init; }
    public required Phone Phone { get; init; }
    public required UserStatus Status { get; init; }
    public required UserRole Role { get; init; }
}

public sealed class GetUsersQueryValidator : AbstractValidator<GetUsersQuery>
{
    public GetUsersQueryValidator(
        IValidator<PaginatedQuery> baseValidator)
    {
        Include(baseValidator);

        RuleFor(query => query.Id)
            .NotEmpty()
            .When(query => query.Id != null);
        RuleFor(query => query.Firstname)
            .NotEmpty()
            .When(query => query.Firstname != null);
        RuleFor(query => query.Lastname)
            .NotEmpty()
            .When(query => query.Lastname != null);        
        RuleFor(query => query.Email)
            .NotEmpty()
            .When(query => query.Email != null);
        RuleFor(query => query.Username)
            .NotEmpty()
            .When(query => query.Username != null);
        RuleFor(query => query.City)
            .NotEmpty()
            .When(query => query.City != null);
        RuleFor(query => query.Street)
            .NotEmpty()
            .When(query => query.Street != null);
        RuleFor(query => query.Number)
            .NotEmpty()
            .When(query => query.Number != null);
        RuleFor(query => query.Zipcode)
            .NotEmpty()
            .When(query => query.Zipcode != null);
        RuleFor(query => query.Lat)
            .NotEmpty()
            .When(query => query.Lat != null);
        RuleFor(query => query.Long)
            .NotEmpty()
            .When(query => query.Long != null);
        RuleFor(query => query.Phone)
            .NotEmpty()
            .When(query => query.Phone != null);
        RuleFor(query => query.Status)
            .NotEmpty()
            .When(query => query.Status != null);
        RuleFor(query => query.Role)
            .NotEmpty()
            .When(query => query.Role != null);
    }
}

public sealed class GetUsersProfile : Profile
{
    public GetUsersProfile()
    {
        CreateMap<User, GetUsersQuerySummary>();
        CreateMap<GetUsersQuery, GetUsersQueryFilter>()
            .ForMember(filter => filter.CurrentPage, expression => expression.MapFrom(query => query.CurrentPage))
            .ForMember(filter => filter.PageSize, expression => expression.MapFrom(query => query.PageSize))
            .ForMember(filter => filter.OrderBy, expression => expression.MapFrom(query => query.OrderBy));

        CreateMap<PaginatedList<User>, GetUsersQueryResult>()
            .ForMember(queryResult => queryResult.Users, opt => opt.MapFrom(users => users))
            .ForMember(queryResult => queryResult.TotalCount, opt => opt.MapFrom(users => users.Count))
            .ForMember(queryResult => queryResult.Page, opt => opt.MapFrom(users => users.CurrentPage))
            .ForMember(queryResult => queryResult.PageSize, opt => opt.MapFrom(users => users.PageSize))
            .ForMember(queryResult => queryResult.TotalPages, opt => opt.MapFrom(users => users.TotalPages));
    }
}

public sealed class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, ApplicationResult<GetUsersQueryResult>>
{
    private readonly IMapper _mapper;
    private readonly IUserRepository _userRepository;

    public GetUsersQueryHandler(
        IMapper mapper,
        IUserRepository userRepository)
    {
        _mapper = mapper;
        _userRepository = userRepository;
    }

    public async Task<ApplicationResult<GetUsersQueryResult>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        var queryContract = _mapper.Map<GetUsersQueryFilter>(request);

        var users = await _userRepository.GetUsersAsync(queryContract, cancellationToken);

        var result = _mapper.Map<GetUsersQueryResult>(users);

        return result;
    }
}

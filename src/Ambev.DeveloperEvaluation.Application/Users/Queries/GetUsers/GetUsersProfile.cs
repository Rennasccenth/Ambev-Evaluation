using Ambev.DeveloperEvaluation.Domain.Aggregates.Users;
using Ambev.DeveloperEvaluation.Domain.Aggregates.Users.Repositories;
using Ambev.DeveloperEvaluation.Domain.Common;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Users.Queries.GetUsers;

public sealed class GetUsersProfile : Profile
{
    public GetUsersProfile()
    {
        // In
        CreateMap<GetUsersQuery, GetUsersQueryFilter>()
            .ForMember(filter => filter.CurrentPage, expression => expression.MapFrom(query => query.CurrentPage))
            .ForMember(filter => filter.PageSize, expression => expression.MapFrom(query => query.PageSize))
            .ForMember(filter => filter.OrderBy, expression => expression.MapFrom(query => query.OrderBy))
            .ForMember(filter => filter.FilterBy, expression => expression.MapFrom(query => query.FilterBy));

        // Out
        CreateMap<User, GetUsersQuerySummary>();

        CreateMap<PaginatedList<User>, GetUsersQueryResult>()
            .ForMember(queryResult => queryResult.Users, opt => opt.MapFrom(items => items))
            .ForMember(queryResult => queryResult.TotalCount, opt => opt.MapFrom(users => users.Count))
            .ForMember(queryResult => queryResult.Page, opt => opt.MapFrom(users => users.CurrentPage))
            .ForMember(queryResult => queryResult.PageSize, opt => opt.MapFrom(users => users.PageSize))
            .ForMember(queryResult => queryResult.TotalPages, opt => opt.MapFrom(users => users.TotalPages));
    }
}
using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories.User;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Users.Queries.GetUsers;

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
            .ForMember(queryResult => queryResult, opt => opt.MapFrom(users => users))
            .ForMember(queryResult => queryResult.TotalCount, opt => opt.MapFrom(users => users.Count))
            .ForMember(queryResult => queryResult.Page, opt => opt.MapFrom(users => users.CurrentPage))
            .ForMember(queryResult => queryResult.PageSize, opt => opt.MapFrom(users => users.PageSize))
            .ForMember(queryResult => queryResult.TotalPages, opt => opt.MapFrom(users => users.TotalPages));
    }
}
using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories.Products;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Products.Queries.GetProducts;

public class GetProductsProfile : Profile
{
    public GetProductsProfile()
    {
        CreateMap<GetProductsQuery, GetProductsQueryFilter>()
            .ForMember(dest => dest.FilterBy, expression => expression.MapFrom(src => src.FilterBy))
            .ForMember(dest => dest.OrderBy, expression => expression.MapFrom(src => src.OrderBy))
            .ForMember(dest => dest.CurrentPage, opt => opt.MapFrom(src => src.CurrentPage))
            .ForMember(dest => dest.PageSize, opt => opt.MapFrom(src => src.PageSize));

        CreateMap<Product, GetProductsQueryResultItem>();
        CreateMap<PaginatedList<Product>, GetProductsQueryResult>()
            .ForMember(dest => dest.Products, opt => opt.MapFrom(src => src))
            .ForMember(dest => dest.CurrentPage, opt => opt.MapFrom(src => src.CurrentPage))
            .ForMember(dest => dest.TotalItems, opt => opt.MapFrom(src => src.TotalItems))
            .ForMember(dest => dest.PageSize, opt => opt.MapFrom(src => src.PageSize));
    }
}
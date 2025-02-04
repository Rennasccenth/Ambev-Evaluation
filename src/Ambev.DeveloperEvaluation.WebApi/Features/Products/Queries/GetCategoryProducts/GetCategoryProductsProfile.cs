using Ambev.DeveloperEvaluation.Application.Products.Queries.GetProducts;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.Queries.GetCategoryProducts;

public class GetCategoryProductsProfile : Profile
{
    public GetCategoryProductsProfile()
    {
        // In
        CreateMap<GetCategoryProductsRequest, GetProductsQuery>()
            .ForMember(dest => dest.CurrentPage, opt => opt.MapFrom(src => src.CurrentPage))
            .ForMember(dest => dest.PageSize, opt => opt.MapFrom(src => src.PageSize))
            .ForMember(dest => dest.FilterBy, opt => opt.MapFrom(src => src.FilterBy))
            .ForMember(dest => dest.OrderBy, opt => opt.MapFrom(src => src.OrderBy));

        // Out
        CreateMap<GetProductsQueryResultItem, GetCategoryProductsResponseItem>();
        CreateMap<GetProductsQueryResult, GetCategoryProductsResponse>()
            .ForMember(dest => dest.Data, opt => opt.MapFrom(src => src.Products))
            .ForMember(dest => dest.TotalCount, opt => opt.MapFrom(src => src.TotalItems))
            .ForMember(dest => dest.PageNumber, opt => opt.MapFrom(src => src.CurrentPage))
            .ForMember(dest => dest.PageSize, opt => opt.MapFrom(src => src.PageSize))
            .ForMember(dest => dest.TotalPages, opt => opt.MapFrom(src => src.TotalPages));
    }
}
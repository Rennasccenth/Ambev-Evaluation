using Ambev.DeveloperEvaluation.Application.Products.Queries.GetCategories;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.Queries.GetCategories;

public sealed class GetCategoriesProfile : Profile
{
    public GetCategoriesProfile()
    {
        CreateMap<GetCategoriesQueryResponse, GetCategoriesResponse>()
            .ConstructUsing(src => new GetCategoriesResponse(src.Categories));
    }
}
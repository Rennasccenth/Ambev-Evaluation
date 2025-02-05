using Ambev.DeveloperEvaluation.Domain.Aggregates.Products;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Products.Queries.GetProduct;

public sealed class GetProductProfile : Profile
{
    public GetProductProfile()
    {
        CreateMap<Product, GetProductQueryResult>();
    }
}
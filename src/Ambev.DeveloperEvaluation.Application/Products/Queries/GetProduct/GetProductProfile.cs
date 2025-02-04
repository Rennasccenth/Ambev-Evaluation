using Ambev.DeveloperEvaluation.Domain.Entities;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Products.Queries.GetProduct;

public sealed class GetProductProfile : Profile
{
    public GetProductProfile()
    {
        CreateMap<Product, GetProductQueryResult>();
    }
}
using Ambev.DeveloperEvaluation.Domain.Aggregates.Products;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Products.Commands.UpdateProduct;

public sealed class UpdateProductProfile : Profile
{
    public UpdateProductProfile()
    {
        CreateMap<Product, UpdateProductCommandResult>();
    }
}
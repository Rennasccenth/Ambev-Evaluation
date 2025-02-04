using AutoMapper;
using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Application.Products.Commands.UpdateProduct;

public sealed class UpdateProductProfile : Profile
{
    public UpdateProductProfile()
    {
        CreateMap<Product, UpdateProductCommandResult>();
    }
}
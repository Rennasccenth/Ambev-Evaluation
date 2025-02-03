using Ambev.DeveloperEvaluation.Domain.Entities;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Products.Commands.CreateProduct;

public class CreateProductProfile : Profile
{
    public CreateProductProfile()
    {
        // Out
        CreateMap<Product, CreateProductCommandResult>();
    }    
}
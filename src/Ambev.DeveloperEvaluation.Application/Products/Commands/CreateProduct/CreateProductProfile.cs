using Ambev.DeveloperEvaluation.Domain.Aggregates.Products;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Products.Commands.CreateProduct;

public class CreateProductProfile : Profile
{
    public CreateProductProfile()
    {
        // Out
        CreateMap<Product, CreateProductCommandResult>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id));
    }    
}
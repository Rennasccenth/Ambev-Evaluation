using AutoMapper;
using Ambev.DeveloperEvaluation.Application.Products.Commands.CreateProduct;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.Commands.CreateProduct
{
    public class CreateProductProfile : Profile
    {
        public CreateProductProfile()
        {
            CreateMap<CreateProductRequest, CreateProductCommand>();
            CreateMap<CreateProductCommandResult, CreateProductResponse>();
        }
    }
}
using AutoMapper;
using Ambev.DeveloperEvaluation.Application.Products.Commands.CreateProduct;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.Commands.CreateProduct
{
    public class CreateProductProfile : Profile
    {
        public CreateProductProfile()
        {
            // In
            CreateMap<CreateProductRequest, CreateProductCommand>();
            
            // Out
            CreateMap<CreateProductCommandResult, CreateProductResponse>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id));
        }
    }
}
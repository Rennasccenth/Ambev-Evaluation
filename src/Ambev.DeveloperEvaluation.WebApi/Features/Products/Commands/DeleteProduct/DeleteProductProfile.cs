using Ambev.DeveloperEvaluation.Application.Products.Commands.DeleteProduct;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.Commands.DeleteProduct;

public sealed class DeleteProductProfile : Profile
{
    public DeleteProductProfile()
    {
        CreateMap<DeleteProductRequest, DeleteProductCommand>();
    }
}
using Ambev.DeveloperEvaluation.Application.Carts.Commands.DeleteCart;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts.Commands.DeleteCart;

public class DeleteCartProfile : Profile
{
    public DeleteCartProfile()
    {
        // In
        CreateMap<DeleteCartRequest, DeleteCartCommand>();
    }
}
using Ambev.DeveloperEvaluation.Application.Users.Commands.DeleteUserCart;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Users.Commands.DeleteUserCart;

public sealed class DeleteUserCartProfile : Profile
{
    public DeleteUserCartProfile()
    {
        // In
        CreateMap<DeleteUserCartRequest, DeleteUserCartCommand>();
    }
}
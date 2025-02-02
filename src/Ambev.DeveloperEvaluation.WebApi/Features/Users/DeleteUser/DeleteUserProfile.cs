using Ambev.DeveloperEvaluation.Application.Users.Commands.DeleteUser;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Users.DeleteUser;

public sealed class DeleteUserProfile : Profile
{
    public DeleteUserProfile()
    {
        CreateMap<DeleteUserRequest, DeleteUserCommand>()
            .ConstructUsing(request => new DeleteUserCommand(request.Id));
    }
}

using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Users.DeleteUser;

public sealed class DeleteUserProfile : Profile
{
    public DeleteUserProfile()
    {
        CreateMap<DeleteUserRequest, Application.Users.DeleteUser.DeleteUserCommand>()
            .ConstructUsing(request => new Application.Users.DeleteUser.DeleteUserCommand(request.Id));
    }
}

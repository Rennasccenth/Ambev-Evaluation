using Ambev.DeveloperEvaluation.Domain.Entities;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Users.Commands.UpdateUser;

public sealed class UpdateUserProfile : Profile
{
    public UpdateUserProfile()
    {
        CreateMap<User, UpdateUserCommandResult>()
            .ForPath(result => result.Name.Firstname, expression => expression.MapFrom(user => user.Firstname))
            .ForPath(result => result.Name.Lastname, expression => expression.MapFrom(user => user.Lastname));
    }
}
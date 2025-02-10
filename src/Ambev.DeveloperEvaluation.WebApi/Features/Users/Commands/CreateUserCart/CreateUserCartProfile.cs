using Ambev.DeveloperEvaluation.Application.Users.Commands.CreateUserCart;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Users.Commands.CreateUserCart;

public sealed class CreateUserCartProfile : Profile
{
    public CreateUserCartProfile()
    {
        // In
        CreateMap<CreateUserCartRequest, CreateUserCartCommand>()
            .ForMember(x => x.UserId, expression => expression.MapFrom(x => x.UserId))
            .ForMember(x => x.Date, expression => expression.MapFrom(x => x.Date))
            .ForMember(x => x.Products, expression => expression.MapFrom(x => x.Products));
    }
}
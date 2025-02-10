using Ambev.DeveloperEvaluation.Application.Carts.Commands.CreateCart;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts.Commands.CreateCart;

public sealed class CreateCartProfile : Profile
{
    public CreateCartProfile()
    {
        // In
        CreateMap<CreateCartRequest, CreateCartCommand>()
            .ForMember(x => x.Date, expression => expression.MapFrom(x => x.Date))
            .ForMember(x => x.Products, expression => expression.MapFrom(x => x.Products));
    }
}
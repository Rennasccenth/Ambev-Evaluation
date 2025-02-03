using System.Globalization;
using Ambev.DeveloperEvaluation.Application.Products.Commands.UpdateProduct;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.Commands.UpdateProduct;

public sealed class UpdateProductProfile : Profile
{
    public UpdateProductProfile()
    {
        CreateMap<UpdateProductRequest, UpdateProductCommand>()
            .ConstructUsing(request => 
                new UpdateProductCommand(request.Id, request.Title, request.Price, request.Description, request.Category, request.Image, request.Rating));

        CreateMap<UpdateProductCommandResult, UpdateProductResponse>()
            .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price.ToString(CultureInfo.InvariantCulture)));
    }
}
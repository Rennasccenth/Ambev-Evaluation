using Ambev.DeveloperEvaluation.Domain.Aggregates.Sales;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Sales.Common;

public sealed class SaleResultProfile : Profile
{
    public SaleResultProfile()
    {
        // Out
        CreateMap<Sale, SaleResult>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.CustomerId, opt => opt.MapFrom(src => src.CustomerId))
            .ForMember(dest => dest.Number, opt => opt.MapFrom(src => src.Number))
            .ForMember(dest => dest.Branch, opt => opt.MapFrom(src => src.Branch))
            .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => src.CreatedDate))
            .ForMember(dest => dest.TerminationDate, opt => opt.MapFrom(src => src.TerminationDate))
            .ForMember(dest => dest.CanceledDate, opt => opt.MapFrom(src => src.CanceledDate))
            .ForMember(dest => dest.Products, opt => opt.MapFrom(src => src.Products));

        CreateMap<SaleProduct, SaleProductResult>()
            .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductId))
            .ForMember(dest => dest.SaleId, opt => opt.MapFrom(src => src.SaleId))
            .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity))
            .ForMember(dest => dest.UnitPrice, opt => opt.MapFrom(src => src.UnitPrice))
            .ForMember(dest => dest.TotalPrice, opt => opt.MapFrom(src => src.TotalPrice));
    }
}
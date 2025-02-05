using Ambev.DeveloperEvaluation.WebApi.Features.Carts.Common;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts.Commands.UpsertCart;

public sealed class UpsertCartRequest
{
    public Guid UserId { get; set; }    
    public required DateTime Date { get; set; }
    public required List<ProductQuantifier> Products { get; set; }
}
using Ambev.DeveloperEvaluation.WebApi.Features.Carts.Common;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts.Commands.CreateCart;

public sealed class CreateCartRequest
{
    public required DateTime Date { get; set; }
    public required List<ProductQuantifier> Products { get; init; } = [];
}
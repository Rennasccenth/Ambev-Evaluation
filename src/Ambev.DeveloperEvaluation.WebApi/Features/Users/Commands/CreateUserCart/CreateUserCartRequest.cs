using Ambev.DeveloperEvaluation.WebApi.Features.Carts.Common;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Users.Commands.CreateUserCart;

public sealed class CreateUserCartRequest
{
    public Guid UserId { get; }
    public DateTime Date { get; }
    public List<ProductQuantifier> Products { get; }

    public CreateUserCartRequest(Guid userId, DateTime date, List<ProductQuantifier> products)
    {
        UserId = userId;
        Date = date;
        Products = products;
    }
}

public sealed class CreateUserCartRequestSummary
{
    public required DateTime Date { get; set; }
    public required List<ProductQuantifier> Products { get; init; }
}
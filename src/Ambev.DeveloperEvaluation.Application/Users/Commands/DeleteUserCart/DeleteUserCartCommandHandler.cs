using Ambev.DeveloperEvaluation.Common.Results;
using Ambev.DeveloperEvaluation.Domain.Aggregates.Carts.Services;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Users.Commands.DeleteUserCart;

public sealed class DeleteUserCartCommandHandler 
    : IRequestHandler<DeleteUserCartCommand, ApplicationResult<DeleteUserCartCommandResult>>
{
    private readonly ICartsService _cartsService;
    private readonly ILogger<DeleteUserCartCommandHandler> _logger;

    public DeleteUserCartCommandHandler(ICartsService cartsService, ILogger<DeleteUserCartCommandHandler> logger)
    {
        _cartsService = cartsService;
        _logger = logger;
    }
    
    public async Task<ApplicationResult<DeleteUserCartCommandResult>> Handle(DeleteUserCartCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Deleting cart for user {UserId}", request.UserId);
        
        var deletedCartResult = await _cartsService.DeleteCartByUserIdAsync(request.UserId, cancellationToken);
        return deletedCartResult.Match<ApplicationResult<DeleteUserCartCommandResult>>(
            _ => new DeleteUserCartCommandResult(true),
            error => error
        );
    }
}
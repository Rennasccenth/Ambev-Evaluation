using Ambev.DeveloperEvaluation.Common.Errors;
using Ambev.DeveloperEvaluation.Common.Results;
using Ambev.DeveloperEvaluation.Common.Security;
using Ambev.DeveloperEvaluation.Domain.Abstractions;
using Ambev.DeveloperEvaluation.Domain.Aggregates.Carts.Services;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Carts.Commands.DeleteCart;

public sealed class DeleteCartCommandHandler : IRequestHandler<DeleteCartCommand, ApplicationResult<DeleteCartCommandResult>>
{
    private readonly ICartsService _cartsService;
    private readonly IUserContext _userContext;
    private readonly ILogger<DeleteCartCommandHandler> _logger;

    public DeleteCartCommandHandler(
        ICartsService cartsService,
        IUserContext userContext,
        ILogger<DeleteCartCommandHandler> logger)
    {
        _cartsService = cartsService;
        _userContext = userContext;
        _logger = logger;
    }

    public async Task<ApplicationResult<DeleteCartCommandResult>> Handle(DeleteCartCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Deleting cart {CartId}", request.CartId);

        if (_userContext.IsAuthenticated)
        {
            return ApplicationError.BadRequestError("User is already authenticated, delete the cart by calling the proper endpoint.");
        }

        var deleteCartResult = await _cartsService.DeleteCartAsync(request.CartId, cancellationToken);
        return deleteCartResult.Match<ApplicationResult<DeleteCartCommandResult>>(
            _ => new DeleteCartCommandResult(true),
            error => error
        );
    }
}
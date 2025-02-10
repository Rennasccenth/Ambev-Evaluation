using Ambev.DeveloperEvaluation.Application.Carts.Commands.CreateCart;
using Ambev.DeveloperEvaluation.Application.Carts.Commands.DeleteCart;
using Ambev.DeveloperEvaluation.Application.Carts.Commands.UpdateCart;
using Ambev.DeveloperEvaluation.Application.Carts.Queries.GetCart;
using Ambev.DeveloperEvaluation.WebApi.Common;
using Ambev.DeveloperEvaluation.WebApi.Features.Carts.Commands.CreateCart;
using Ambev.DeveloperEvaluation.WebApi.Features.Carts.Commands.DeleteCart;
using Ambev.DeveloperEvaluation.WebApi.Features.Carts.Commands.UpdateCart;
using Ambev.DeveloperEvaluation.WebApi.Features.Carts.Common;
using Ambev.DeveloperEvaluation.WebApi.Features.Carts.Queries.GetCart;
using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts;

[ApiController]
[Route("api/[controller]")]
public class CartsController : BaseController
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public CartsController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [HttpGet("{cartId:guid}")]
    [ProducesResponseType(typeof(CartResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [EndpointSummary("Get a cart - [Public Access]")]
    [EndpointDescription("Gets a cart by their unique identifier.")]
    [OpenApiOperation("Get a cart", "Gets a cart by their unique identifier.")]
    public async Task<IActionResult> GetByCartId(
        Guid cartId,
        [FromServices] IValidator<GetCartRequest> requestValidator,
        CancellationToken ct)
    {
        GetCartRequest request = new GetCartRequest(cartId);
        ValidationResult? validationResult = await requestValidator.ValidateAsync(request, ct);
        if (!validationResult.IsValid) return HandleKnownError(validationResult.Errors);

        var getCartQuery = _mapper.Map<GetCartQuery>(request);
        var result = await _mediator.Send(getCartQuery, ct);
    
        return result.Match(
            onSuccess: successResult => Ok(_mapper.Map<CartResponse>(successResult)),
            onFailure: HandleKnownError);
    }

    [HttpPost]
    [ProducesResponseType(typeof(CartResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status422UnprocessableEntity)]
    [EndpointSummary("Create a cart - [Public Access]")]
    [EndpointDescription("Creates a cart unbounded to any user.")]
    [OpenApiOperation("Create a cart", "Creates a cart unbounded to any user.")]
    public async Task<IActionResult> CreateCart(
        [FromBody] CreateCartRequest request,
        [FromServices] IValidator<CreateCartRequest> requestValidator,
        CancellationToken ct)
    {
        ValidationResult? validationResult = await requestValidator.ValidateAsync(request, ct);
        if (!validationResult.IsValid) return HandleKnownError(validationResult.Errors);
    
        var command = _mapper.Map<CreateCartCommand>(request);
        var result = await _mediator.Send(command, ct);

        return result.Match(
            onSuccess: successResult => CreatedAtAction(nameof(GetByCartId), new { CartId = successResult.Id },
                _mapper.Map<CartResponse>(successResult)),
            onFailure: HandleKnownError);
    }
    
    [HttpPut("{cartId:guid}")]
    [ProducesResponseType(typeof(CartResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [EndpointSummary("Update a cart - [Public Access]")]
    [EndpointDescription("Updates a cart by their unique identifier.")]
    [OpenApiOperation("Update a cart", "Updates a cart by their unique identifier.")]
    public async Task<IActionResult> UpdateCart(
        [FromRoute] Guid cartId,
        [FromBody] UpdateCartProductsRequest productsRequest,
        [FromServices] IValidator<UpdateCartRequest> requestValidator,
        CancellationToken ct)
    {
        UpdateCartRequest request = new(cartId, productsRequest.Products);
        ValidationResult? validationResult = await requestValidator.ValidateAsync(request, ct);
        if (!validationResult.IsValid) return HandleKnownError(validationResult.Errors);
    
        var command = _mapper.Map<UpdateCartCommand>(request);
        var commandResult = await _mediator.Send(command, ct);
    
        return commandResult.Match(
            onSuccess: successResult => Ok(_mapper.Map<CartResponse>(successResult)),
            onFailure: HandleKnownError);
    }
    
    [HttpDelete("{cartId:guid}")]
    [ProducesResponseType(typeof(CartResponse), StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [EndpointSummary("Delete a cart - [Public Access]")]
    [EndpointDescription("Deletes a cart by their unique identifier.")]
    [OpenApiOperation("Delete a cart", "Deletes a cart by their unique identifier.")]
    public async Task<IActionResult> DeleteCart(
        Guid cartId,
        [FromServices] IValidator<DeleteCartRequest> requestValidator,
        CancellationToken ct)
    {
        DeleteCartRequest request = new(cartId);
        ValidationResult? validationResult = await requestValidator.ValidateAsync(request, ct);
        if (!validationResult.IsValid) return HandleKnownError(validationResult.Errors);
    
        var command = _mapper.Map<DeleteCartCommand>(request);
        var result = await _mediator.Send(command, ct);
    
        return result.Match(
            onSuccess: _ => NoContent(),
            onFailure: HandleKnownError);
    }
}
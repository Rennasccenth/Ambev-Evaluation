using Ambev.DeveloperEvaluation.Application.Carts.Commands;
using Ambev.DeveloperEvaluation.Application.Carts.Queries;
using Ambev.DeveloperEvaluation.WebApi.Common;
using Ambev.DeveloperEvaluation.WebApi.Features.Carts.Commands.UpsertCart;
using Ambev.DeveloperEvaluation.WebApi.Features.Carts.Common;
using Ambev.DeveloperEvaluation.WebApi.Features.Carts.Queries.GetCart;
using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Mvc;

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

    [HttpGet("{userId:guid}")]
    [ProducesResponseType(typeof(CartResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(
        Guid userId,
        [FromServices] IValidator<GetCartRequest> requestValidator,
        CancellationToken ct)
    {
        GetCartRequest request = new GetCartRequest(userId);
        ValidationResult? validationResult = await requestValidator.ValidateAsync(request, ct);
        if (!validationResult.IsValid) return HandleKnownError(validationResult.Errors);

        var getCartQuery = _mapper.Map<GetCartQuery>(request);
        var result = await _mediator.Send(getCartQuery, ct);
    
        return result.Match(
            onSuccess: successResult => Ok(_mapper.Map<CartResponse>(successResult)),
            onFailure: HandleKnownError);
    }

    [HttpPut]
    [ProducesResponseType(typeof(CartResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Upsert(
        [FromBody] UpsertCartRequest request,
        [FromServices] IValidator<UpsertCartRequest> requestValidator,
        CancellationToken ct)
    {
        ValidationResult? validationResult = await requestValidator.ValidateAsync(request, ct);
        if (!validationResult.IsValid) return HandleKnownError(validationResult.Errors);
    
        var command = _mapper.Map<UpsertCartCommand>(request);
        var result = await _mediator.Send(command, ct);

        return result.Match(
            onSuccess: successResult => CreatedAtAction(nameof(GetById), new { successResult.UserId },
                _mapper.Map<CartResponse>(successResult)),
            onFailure: HandleKnownError);
    }
}
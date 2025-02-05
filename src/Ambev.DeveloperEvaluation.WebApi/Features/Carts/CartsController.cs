using Ambev.DeveloperEvaluation.Application.Carts.Commands;
using Ambev.DeveloperEvaluation.Application.Carts.Queries;
using Ambev.DeveloperEvaluation.WebApi.Common;
using Ambev.DeveloperEvaluation.WebApi.Features.Carts.Commands.CreateCart;
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

    // [HttpGet("")]
    // [ProducesResponseType(typeof(GetProductsResponse), StatusCodes.Status200OK)]
    // [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    // public async Task<IActionResult> GetAll(
    //     [FromQuery] GetProductsRequest request,
    //     [FromServices] IValidator<GetProductsRequest> requestValidator,
    //     CancellationToken ct)
    // {
    //     request.SetFilter(HttpContext.Request.Query);
    //     var validateAsync = await requestValidator.ValidateAsync(request, ct);
    //     if (!validateAsync.IsValid) return HandleKnownError(validateAsync.Errors);
    //
    //     var getProductQuery = _mapper.Map<GetProductsQuery>(request);
    //     var queryResult = await _mediator.Send(getProductQuery, ct);
    //
    //     return queryResult.Match(
    //         onSuccess: successResult => Ok(_mapper.Map<GetProductsResponse>(successResult)),
    //         onFailure: HandleKnownError);
    // }

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

    [HttpPost]
    [ProducesResponseType(typeof(CartResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Create(
        [FromBody] CreateCartRequest request,
        [FromServices] IValidator<CreateCartRequest> requestValidator,
        CancellationToken ct)
    {
        ValidationResult? validationResult = await requestValidator.ValidateAsync(request, ct);
        if (!validationResult.IsValid) return HandleKnownError(validationResult.Errors);
    
        var command = _mapper.Map<CreateCartCommand>(request);
        var result = await _mediator.Send(command, ct);

        return result.Match(
            onSuccess: successResult => CreatedAtAction(nameof(GetById), new { successResult.UserId },
                _mapper.Map<CartResponse>(successResult)),
            onFailure: HandleKnownError);
    }

    // [HttpPut("{id:guid}")]
    // [ProducesResponseType(typeof(UpdateProductResponse), StatusCodes.Status200OK)]
    // [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    // [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    // public async Task<IActionResult> Update(
    //     [FromRoute] Guid id,
    //     [FromBody] UpdateProductRequest request,
    //     [FromServices] IValidator<UpdateProductRequest> requestValidator,
    //     CancellationToken cancellationToken)
    // {
    //     request.Id = id;
    //     ValidationResult? validationResult = await requestValidator.ValidateAsync(request, cancellationToken);
    //     if (!validationResult.IsValid) return HandleKnownError(validationResult.Errors);
    //     var commandResult = await _mediator.Send(_mapper.Map<UpdateProductCommand>(request), cancellationToken);
    //
    //     return commandResult.Match(
    //         onSuccess: successResult => Ok(_mapper.Map<UpdateProductResponse>(successResult)),
    //         onFailure: HandleKnownError);
    // }

    // [HttpDelete("{id:guid}")]
    // [ProducesResponseType(StatusCodes.Status204NoContent)]
    // [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    // public async Task<IActionResult> Delete(
    //     [FromRoute] Guid id,
    //     [FromServices] IValidator<DeleteProductRequest> requestValidator,
    //     CancellationToken ct)
    // {
    //     DeleteProductRequest request = new(id);
    //     ValidationResult validationResult = await requestValidator.ValidateAsync(request, ct);
    //     if (!validationResult.IsValid) return HandleKnownError(validationResult.Errors);
    //
    //     var commandResult = await _mediator.Send(_mapper.Map<DeleteProductCommand>(request), ct);
    //
    //     return commandResult.Match(
    //         onSuccess: _ => NoContent(),
    //         onFailure: HandleKnownError);
    // }

    // [HttpGet("categories")]
    // [ProducesResponseType(typeof(GetCategoriesResponse), StatusCodes.Status200OK)]
    // [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    // public async Task<IActionResult> GetAllCategories(CancellationToken ct)
    // {
    //     ApplicationResult<GetCategoriesQueryResponse> queryResult = await _mediator.Send(new GetCategoriesQuery(), ct);
    //
    //     return queryResult.Match(
    //         onSuccess: successResult => Ok(_mapper.Map<GetCategoriesResponse>(successResult)),
    //         onFailure: HandleKnownError);
    // }

    // [HttpGet("categories/{category}")]
    // [ProducesResponseType(typeof(GetProductsResponse), StatusCodes.Status200OK)]
    // [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    // public async Task<IActionResult> GetFilteredByCategory(
    //     [FromRoute] string category,
    //     [FromServices] IValidator<GetProductsRequest> requestValidator,
    //     CancellationToken ct)
    // {
    //     GetProductsRequest request = new();
    //     request.SetFilter(HttpContext.Request.Query);
    //     request.FilterBy["category"] = category;
    //
    //     var validateAsync = requestValidator.ValidateAsync(request, ct);
    //     if (!validateAsync.Result.IsValid) return HandleKnownError(validateAsync.Result.Errors);
    //
    //     var getProductsQuery = _mapper.Map<GetProductsQuery>(request);
    //     ApplicationResult<GetProductsQueryResult> queryResult = await _mediator.Send(getProductsQuery, ct);
    //
    //     return queryResult.Match(
    //         onSuccess: successResult => Ok(_mapper.Map<GetCategoryProductsResponse>(successResult)),
    //         onFailure: HandleKnownError);
    // }
}
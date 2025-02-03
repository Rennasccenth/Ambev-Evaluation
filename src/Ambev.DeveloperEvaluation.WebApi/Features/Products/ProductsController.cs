using Ambev.DeveloperEvaluation.Application.Products.Commands.CreateProduct;
using Ambev.DeveloperEvaluation.Application.Products.Queries.GetProduct;
using Ambev.DeveloperEvaluation.WebApi.Common;
using Ambev.DeveloperEvaluation.WebApi.Features.Products.Commands.CreateProduct;
using Ambev.DeveloperEvaluation.WebApi.Features.Products.Queries;
using Ambev.DeveloperEvaluation.WebApi.Features.Products.Queries.GetProduct;
using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Products;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : BaseController
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public ProductsController(
        IMediator mediator,
        IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(GetProductResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Get(
        [FromRoute] Guid id,
        [FromServices] IValidator<GetProductRequest> requestValidator,
        CancellationToken ct)
    {
        GetProductRequest request = new GetProductRequest(id);
        ValidationResult? validationResult = await requestValidator.ValidateAsync(request, ct);
        if (!validationResult.IsValid) return HandleKnownError(validationResult.Errors);

        var result = await _mediator.Send(_mapper.Map<GetProductQuery>(request), ct);

        return result.Match(
            onSuccess: successResult => Ok(_mapper.Map<GetProductResponse>(successResult)),
            onFailure: HandleKnownError);
    }

    [HttpPost]
    [ProducesResponseType(typeof(CreateProductResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Create(
        [FromBody] CreateProductRequest request,
        [FromServices] IValidator<CreateProductRequest> createProductValidator,
        CancellationToken ct)
    {
        ValidationResult? validationResult = await createProductValidator.ValidateAsync(request, ct);
        if (!validationResult.IsValid) return HandleKnownError(validationResult.Errors);

        var command = _mapper.Map<CreateProductCommand>(request);
        var result = await _mediator.Send(command, ct);

        return result.Match(
            onSuccess: successResult => CreatedAtAction(nameof(Get), new { successResult.Id }, successResult),
            onFailure: HandleKnownError);
    }
}
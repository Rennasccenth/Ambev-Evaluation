using Ambev.DeveloperEvaluation.Application.Products.Commands.CreateProduct;
using Ambev.DeveloperEvaluation.Application.Products.Commands.UpdateProduct;
using Ambev.DeveloperEvaluation.WebApi.Features.Products.Commands.UpdateProduct;
using Ambev.DeveloperEvaluation.Application.Products.Commands.DeleteProduct;
using Ambev.DeveloperEvaluation.Application.Products.Queries.GetCategories;
using Ambev.DeveloperEvaluation.Application.Products.Queries.GetProduct;
using Ambev.DeveloperEvaluation.Application.Products.Queries.GetProducts;
using Ambev.DeveloperEvaluation.Common.Results;
using Ambev.DeveloperEvaluation.WebApi.Common;
using Ambev.DeveloperEvaluation.WebApi.Features.Products.Commands.CreateProduct;
using Ambev.DeveloperEvaluation.WebApi.Features.Products.Commands.DeleteProduct;
using Ambev.DeveloperEvaluation.WebApi.Features.Products.Queries.GetCategories;
using Ambev.DeveloperEvaluation.WebApi.Features.Products.Queries.GetCategoryProducts;
using Ambev.DeveloperEvaluation.WebApi.Features.Products.Queries.GetProduct;
using Ambev.DeveloperEvaluation.WebApi.Features.Products.Queries.GetProducts;
using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Authorization;
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

    [HttpGet("")]
    [ProducesResponseType(typeof(GetProductsResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetAll(
        [FromQuery] GetProductsRequest request,
        [FromServices] IValidator<GetProductsRequest> requestValidator,
        CancellationToken ct)
    {
        request.SetFilter(HttpContext.Request.Query);
        var validateAsync = await requestValidator.ValidateAsync(request, ct);
        if (!validateAsync.IsValid) return HandleKnownError(validateAsync.Errors);

        var getProductQuery = _mapper.Map<GetProductsQuery>(request);
        var queryResult = await _mediator.Send(getProductQuery, ct);

        return queryResult.Match(
            onSuccess: successResult => Ok(_mapper.Map<GetProductsResponse>(successResult)),
            onFailure: HandleKnownError);
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
    [Authorize]
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
            onSuccess: successResult => CreatedAtAction(nameof(Get), new { successResult.Id },
                _mapper.Map<CreateProductResponse>(successResult)),
            onFailure: HandleKnownError);
    }

    [Authorize]
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(UpdateProductResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(
        [FromRoute] Guid id,
        [FromBody] UpdateProductRequest request,
        [FromServices] IValidator<UpdateProductRequest> requestValidator,
        CancellationToken cancellationToken)
    {
        request.Id = id;
        ValidationResult? validationResult = await requestValidator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid) return HandleKnownError(validationResult.Errors);
        var commandResult = await _mediator.Send(_mapper.Map<UpdateProductCommand>(request), cancellationToken);

        return commandResult.Match(
            onSuccess: successResult => Ok(_mapper.Map<UpdateProductResponse>(successResult)),
            onFailure: HandleKnownError);
    }

    [Authorize]
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(
        [FromRoute] Guid id,
        [FromServices] IValidator<DeleteProductRequest> requestValidator,
        CancellationToken ct)
    {
        DeleteProductRequest request = new(id);
        ValidationResult validationResult = await requestValidator.ValidateAsync(request, ct);
        if (!validationResult.IsValid) return HandleKnownError(validationResult.Errors);

        var commandResult = await _mediator.Send(_mapper.Map<DeleteProductCommand>(request), ct);

        return commandResult.Match(
            onSuccess: _ => NoContent(),
            onFailure: HandleKnownError);
    }

    [HttpGet("categories")]
    [ProducesResponseType(typeof(GetCategoriesResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetAllCategories(CancellationToken ct)
    {
        ApplicationResult<GetCategoriesQueryResponse> queryResult = await _mediator.Send(new GetCategoriesQuery(), ct);

        return queryResult.Match(
            onSuccess: successResult => Ok(_mapper.Map<GetCategoriesResponse>(successResult)),
            onFailure: HandleKnownError);
    }

    [HttpGet("categories/{category}")]
    [ProducesResponseType(typeof(GetProductsResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetFilteredByCategory(
        [FromRoute] string category,
        [FromServices] IValidator<GetProductsRequest> requestValidator,
        CancellationToken ct)
    {
        GetProductsRequest request = new();
        request.SetFilter(HttpContext.Request.Query);
        request.FilterBy["category"] = category;

        var validateAsync = requestValidator.ValidateAsync(request, ct);
        if (!validateAsync.Result.IsValid) return HandleKnownError(validateAsync.Result.Errors);

        var getProductsQuery = _mapper.Map<GetProductsQuery>(request);
        ApplicationResult<GetProductsQueryResult> queryResult = await _mediator.Send(getProductsQuery, ct);

        return queryResult.Match(
            onSuccess: successResult => Ok(_mapper.Map<GetCategoryProductsResponse>(successResult)),
            onFailure: HandleKnownError);
    }
}
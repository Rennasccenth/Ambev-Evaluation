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
using NSwag.Annotations;

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
    [EndpointSummary("Get all products by filter - [Public Access]")]
    [EndpointDescription("Get all products that fulfill the dynamic filter.")]
    [OpenApiOperation("Get all products by filter - [Public Access]", "Get all products that fulfill the dynamic filter.")]
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

    [HttpGet("{productId:guid}")]
    [ProducesResponseType(typeof(GetProductResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [EndpointSummary("Get product by id - [Public Access]")]
    [EndpointDescription("Get a product by its unique identifier.")]
    [OpenApiOperation("Get product by id - [Public Access]", "Get a product by its unique identifier.")]
    public async Task<IActionResult> GetByProductId(
        [FromRoute] Guid productId,
        [FromServices] IValidator<GetProductRequest> requestValidator,
        CancellationToken ct)
    {
        GetProductRequest request = new GetProductRequest(productId);
        ValidationResult? validationResult = await requestValidator.ValidateAsync(request, ct);
        if (!validationResult.IsValid) return HandleKnownError(validationResult.Errors);

        var result = await _mediator.Send(_mapper.Map<GetProductQuery>(request), ct);

        return result.Match(
            onSuccess: successResult => Ok(_mapper.Map<GetProductResponse>(successResult)),
            onFailure: HandleKnownError);
    }

    [HttpPost]
    [Authorize(Roles = "Manager, Admin")]
    [ProducesResponseType(typeof(CreateProductResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    [EndpointSummary("Create product - 🔐 [Only Managers or Admins allowed]")]
    [EndpointDescription("Create a new product in the system.")]
    [OpenApiOperation("Create product - 🔐 [Only Managers or Admins allowed]", "Create a new product in the system.")]
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
            onSuccess: successResult => CreatedAtAction(nameof(GetByProductId), new { ProductId = successResult.Id },
                _mapper.Map<CreateProductResponse>(successResult)),
            onFailure: HandleKnownError);
    }

    [HttpPut("{productId:guid}")]
    [Authorize(Roles = "Manager, Admin")]
    [ProducesResponseType(typeof(UpdateProductResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [EndpointSummary("Update product - 🔐 [Only Managers or Admins allowed]")]
    [EndpointDescription("Update an existing product in the system.")]
    [OpenApiOperation("Update product - 🔐 [Only Managers or Admins allowed]", "Update an existing product in the system.")]
    public async Task<IActionResult> Update(
        [FromRoute] Guid productId,
        [FromBody] UpdateProductRequest request,
        [FromServices] IValidator<UpdateProductRequest> requestValidator,
        CancellationToken cancellationToken)
    {
        request.Id = productId;
        ValidationResult? validationResult = await requestValidator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid) return HandleKnownError(validationResult.Errors);
        var commandResult = await _mediator.Send(_mapper.Map<UpdateProductCommand>(request), cancellationToken);

        return commandResult.Match(
            onSuccess: successResult => Ok(_mapper.Map<UpdateProductResponse>(successResult)),
            onFailure: HandleKnownError);
    }

    [HttpDelete("{productId:guid}")]
    [Authorize(Roles = "Manager, Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [EndpointSummary("Delete product - 🔐 [Only Managers or Admins allowed]")]
    [EndpointDescription("Delete an existing product in the system.")]
    [OpenApiOperation("Delete product - 🔐 [Only Managers or Admins allowed]", "Delete an existing product in the system.")]
    public async Task<IActionResult> Delete(
        [FromRoute] Guid productId,
        [FromServices] IValidator<DeleteProductRequest> requestValidator,
        CancellationToken ct)
    {
        DeleteProductRequest request = new(productId);
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
    [EndpointSummary("Get all product categories - [Public Access]")]
    [EndpointDescription("Get all possible categories for current registered products.")]
    [OpenApiOperation("Get all categories - [Public Access]", "Get all possible categories for current registered products.")]
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
    [EndpointSummary("Get all products by category - [Public Access]")]
    [EndpointDescription("Get all products on a given category that fulfill the dynamic filter.")]
    [OpenApiOperation("Get all products by category - [Public Access]", "Get all products on a given category that fulfill the dynamic filter.")]
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
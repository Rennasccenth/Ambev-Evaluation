using Ambev.DeveloperEvaluation.Application.Sales.Queries;
using Ambev.DeveloperEvaluation.WebApi.Common;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.Common;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.Queries;
using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales;

[ApiController]
[Route("api/[controller]")]
public class SalesController : BaseController
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public SalesController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [HttpGet("{saleId:guid}")]
    [Authorize(Roles = "Admin, Manager, Customer")]
    [ProducesResponseType(typeof(SaleResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [EndpointSummary("Get a previously created sale - 🔐 [Allowed for any authenticated User]")]
    [EndpointDescription("Gets a Sale, which is created once a Customer user decides to checkout his cart." +
                         "When the user is a customer one, it can't access other sale than the ones he created. [Requires a authenticated user]")]
    [OpenApiOperation("Get a previously created sale - 🔐 [Allowed for any authenticated User]", "Gets a Sale, which is created once a Customer user decides to checkout his cart." +
        "When the user is a customer one, it can't access other sale than the ones he created. [Requires a authenticated user]")]
    public async Task<IActionResult> GetSaleById(
        [FromRoute] Guid saleId,
        [FromServices] IValidator<GetSaleRequest> requestValidator,
        CancellationToken ct)
    {
        GetSaleRequest request = new GetSaleRequest(saleId);
        ValidationResult? validationResult = await requestValidator.ValidateAsync(request, ct);
        if (!validationResult.IsValid) return HandleKnownError(validationResult.Errors);

        var result = await _mediator.Send(_mapper.Map<GetSaleQuery>(request), ct);

        return result.Match(
            onSuccess: successResult => Ok(_mapper.Map<SaleResponse>(successResult)),
            onFailure: HandleKnownError);
    }
}
using Ambev.DeveloperEvaluation.Application.Sales.Common;
using Ambev.DeveloperEvaluation.Common.Results;
using Ambev.DeveloperEvaluation.Domain.Aggregates.Sales.Services;
using AutoMapper;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.Commands.ConcludeSale;

public sealed class ConcludeSaleCommandHandler : IRequestHandler<ConcludeSaleCommand, ApplicationResult<SaleResult>>
{
    private readonly ISalesService _salesService;
    private readonly IMapper _mapper;

    public ConcludeSaleCommandHandler(ISalesService salesService, IMapper mapper)
    {
        _salesService = salesService;
        _mapper = mapper;
    }

    public async Task<ApplicationResult<SaleResult>> Handle(ConcludeSaleCommand request, CancellationToken cancellationToken)
    {
        var concludeSaleResult = await _salesService.ConcludeSaleAsync(request.SaleId, cancellationToken);
        return concludeSaleResult.Match<ApplicationResult<SaleResult>>(
            onSuccess: sale => _mapper.Map<SaleResult>(sale),
            onFailure: error => error);
    }
}
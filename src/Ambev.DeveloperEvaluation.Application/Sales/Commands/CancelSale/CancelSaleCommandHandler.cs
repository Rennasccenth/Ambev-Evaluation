using Ambev.DeveloperEvaluation.Application.Sales.Common;
using Ambev.DeveloperEvaluation.Common.Results;
using Ambev.DeveloperEvaluation.Domain.Aggregates.Sales.Services;
using AutoMapper;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.Commands.CancelSale;

public sealed class CancelSaleCommandHandler : IRequestHandler<CancelSaleCommand, ApplicationResult<SaleResult>>
{
    private readonly ISalesService _salesService;
    private readonly IMapper _mapper;

    public CancelSaleCommandHandler(ISalesService salesService, IMapper mapper)
    {
        _salesService = salesService;
        _mapper = mapper;
    }
    
    public async Task<ApplicationResult<SaleResult>> Handle(CancelSaleCommand request, CancellationToken cancellationToken)
    {
        var canceledSaleResult = await _salesService.CancelSaleAsync(request.SaleId, cancellationToken);

        return canceledSaleResult.Match<ApplicationResult<SaleResult>>(
            sale => _mapper.Map<SaleResult>(sale),
            error => error);
    }
}
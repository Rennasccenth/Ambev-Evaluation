using Ambev.DeveloperEvaluation.Application.Sales.Common;
using Ambev.DeveloperEvaluation.Common.Errors;
using Ambev.DeveloperEvaluation.Common.Results;
using Ambev.DeveloperEvaluation.Domain.Aggregates.Sales;
using Ambev.DeveloperEvaluation.Domain.Aggregates.Sales.Repositories;
using AutoMapper;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.Queries;

public sealed class GetSaleQueryHandler : IRequestHandler<GetSaleQuery, ApplicationResult<SaleResult>>
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;

    public GetSaleQueryHandler(ISaleRepository saleRepository, IMapper mapper)
    {
        _saleRepository = saleRepository;
        _mapper = mapper;
    }

    public async Task<ApplicationResult<SaleResult>> Handle(GetSaleQuery request, CancellationToken cancellationToken)
    {
        Sale? sale = await _saleRepository.FindByIdAsync(request.SaleId, cancellationToken);
        if (sale is null)
        {
            return ApplicationError.NotFoundError($"Sale {request.SaleId} not found");
        }

        return _mapper.Map<SaleResult>(sale);
    }
}
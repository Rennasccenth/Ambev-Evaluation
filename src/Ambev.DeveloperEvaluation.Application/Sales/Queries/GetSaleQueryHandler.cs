using Ambev.DeveloperEvaluation.Application.Sales.Common;
using Ambev.DeveloperEvaluation.Common.Errors;
using Ambev.DeveloperEvaluation.Common.Results;
using Ambev.DeveloperEvaluation.Domain.Abstractions;
using Ambev.DeveloperEvaluation.Domain.Aggregates.Sales;
using Ambev.DeveloperEvaluation.Domain.Aggregates.Sales.Repositories;
using Ambev.DeveloperEvaluation.Domain.Aggregates.Users.Enums;
using AutoMapper;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.Queries;

public sealed class GetSaleQueryHandler : IRequestHandler<GetSaleQuery, ApplicationResult<SaleResult>>
{
    private readonly ISaleRepository _saleRepository;
    private readonly IUserContext _userContext;
    private readonly IMapper _mapper;

    public GetSaleQueryHandler(
        ISaleRepository saleRepository,
        IUserContext userContext,
        IMapper mapper)
    {
        _saleRepository = saleRepository;
        _userContext = userContext;
        _mapper = mapper;
    }

    public async Task<ApplicationResult<SaleResult>> Handle(GetSaleQuery request, CancellationToken cancellationToken)
    {
        Sale? sale = await _saleRepository.FindByIdAsync(request.SaleId, cancellationToken);
        if (sale is null)
        {
            return ApplicationError.NotFoundError($"Sale {request.SaleId} not found");
        }

        if(_userContext.UserRole is UserRole.Customer && _userContext.UserId != sale.CustomerId)
        {
            return ApplicationError.UnauthorizedAccessError("You are not allowed to access this sale.");
        }

        return _mapper.Map<SaleResult>(sale);
    }
}
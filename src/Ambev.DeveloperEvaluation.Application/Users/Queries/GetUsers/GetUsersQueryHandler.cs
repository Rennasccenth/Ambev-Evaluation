using Ambev.DeveloperEvaluation.Common.Results;
using Ambev.DeveloperEvaluation.Domain.Aggregates.Users.Repositories;
using AutoMapper;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Users.Queries.GetUsers;

public sealed class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, ApplicationResult<GetUsersQueryResult>>
{
    private readonly IMapper _mapper;
    private readonly IUserRepository _userRepository;

    public GetUsersQueryHandler(
        IMapper mapper,
        IUserRepository userRepository)
    {
        _mapper = mapper;
        _userRepository = userRepository;
    }

    public async Task<ApplicationResult<GetUsersQueryResult>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        var queryContract = _mapper.Map<GetUsersQueryFilter>(request);

        var users = await _userRepository.GetUsersAsync(queryContract, cancellationToken);

        var result = _mapper.Map<GetUsersQueryResult>(users);

        return result;
    }
}
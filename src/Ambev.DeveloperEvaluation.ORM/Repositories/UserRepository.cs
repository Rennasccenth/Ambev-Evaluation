using System.Linq.Expressions;
using Ambev.DeveloperEvaluation.Common.Results;
using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories.User;
using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.ORM.Repositories;

public class UserRepository : IUserRepository
{
    private readonly DefaultContext _context;

    public UserRepository(DefaultContext context)
    {
        _context = context;
    }

    public async Task<User> CreateAsync(User user, CancellationToken cancellationToken = default)
    {
        await _context.Users.AddAsync(user, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return user;
    }

    public async Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Users.FirstOrDefaultAsync(o=> o.Id == id, cancellationToken);
    }

    public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await _context.Users
            .FirstOrDefaultAsync(u => u.Email == email, cancellationToken);
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entriesDeleted = await _context.Users
            .Where(usr => usr.Id == id)
            .ExecuteDeleteAsync(cancellationToken);

        return entriesDeleted is 1;
    }

    public async Task<User> UpdateAsync(User user, CancellationToken cancellationToken = default)
    {
        _context.Users.Update(user);
        await _context.SaveChangesAsync(cancellationToken);
        return user;
    }

     public async Task<PaginatedList<User>> GetUsersAsync(GetUsersQueryFilter queryFilter, CancellationToken cancellationToken = default)
    {
        IQueryable<User> filteredUsers = ApplyUsersFiltering(_context.Users.AsNoTracking().AsQueryable(), queryFilter);

        if (queryFilter.OrderBy is null)
            return await PaginatedList<User>.CreateAsync(filteredUsers, queryFilter.CurrentPage, queryFilter.PageSize, cancellationToken);

        IOrderedQueryable<User> sortedUsers = ApplySortExpression(filteredUsers, queryFilter.OrderBy);

        return await PaginatedList<User>.CreateAsync(sortedUsers, queryFilter.CurrentPage, queryFilter.PageSize, cancellationToken);
    }

    private static IOrderedQueryable<User> ApplySortExpression(IQueryable<User> filteredUsers, string orderByString)
    {
        var orderingFieldCandidates = orderByString.ToLowerInvariant().Split(',');

        IOrderedQueryable<User> sortingUsers = filteredUsers.OrderBy(usr => 0); // Doesn't modify sorting order.

        foreach (var orderCandidate in orderingFieldCandidates)
        {
            var orderingPair = orderCandidate.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            var orderingKey = orderingPair.FirstOrDefault();
            if (string.IsNullOrEmpty(orderingKey)) continue;

            // Key selector didn't match known ordering expressions
            if (!GetOrderingExpressions.TryGetValue(orderingKey, out var orderingExpression)) continue;

            if (orderingPair.Length == 2)
            {
                var orderingSelector = orderingPair[1];
                sortingUsers = orderingSelector == "desc"
                    ? sortingUsers.ThenByDescending(orderingExpression)
                    : sortingUsers.ThenBy(orderingExpression);
            }
            else
            {
                sortingUsers = sortingUsers.ThenBy(orderingExpression);
            }
        }

        return sortingUsers;
    }

    private static Dictionary<string, Expression<Func<User, object>>> GetOrderingExpressions => new() 
     {
        { "id", u => u.Id }, 
        { "firstname", u => u.Firstname },
        { "lastname", u => u.Lastname },
        { "email", u => u.Email },
        { "username", u => u.Username },
        { "phone", u => u.Phone },
        { "city", u => u.Address.City },
        { "street", u => u.Address.Street },
        { "number", u => u.Address.Number },
        { "zipcode", u => u.Address.ZipCode },
        { "lat", u => u.Address.Latitude },
        { "long", u => u.Address.Longitude },
        { "role", u => u.Role },
        { "status", u => u.Status },
     };

    private static IQueryable<User> ApplyUsersFiltering(IQueryable<User> users, GetUsersQueryFilter queryFilter)
    {
        if (queryFilter.FilterBy.Values.Any())
        {
            users = users.ApplyFilters(queryFilter.FilterBy);
        }

        return users;
    }
}

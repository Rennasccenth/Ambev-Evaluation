using Ambev.DeveloperEvaluation.Domain.Common;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Users.Queries.GetUsers;

public sealed class GetUsersQueryValidator : AbstractValidator<GetUsersQuery>
{
    public GetUsersQueryValidator(
        IValidator<PaginatedQuery> baseValidator)
    {
        Include(baseValidator);

        RuleFor(query => query.Id)
            .NotEmpty()
            .When(query => query.Id != null);
        RuleFor(query => query.Firstname)
            .NotEmpty()
            .When(query => query.Firstname != null);
        RuleFor(query => query.Lastname)
            .NotEmpty()
            .When(query => query.Lastname != null);        
        RuleFor(query => query.Email)
            .NotEmpty()
            .When(query => query.Email != null);
        RuleFor(query => query.Username)
            .NotEmpty()
            .When(query => query.Username != null);
        RuleFor(query => query.City)
            .NotEmpty()
            .When(query => query.City != null);
        RuleFor(query => query.Street)
            .NotEmpty()
            .When(query => query.Street != null);
        RuleFor(query => query.Number)
            .NotEmpty()
            .When(query => query.Number != null);
        RuleFor(query => query.Zipcode)
            .NotEmpty()
            .When(query => query.Zipcode != null);
        RuleFor(query => query.Lat)
            .NotEmpty()
            .When(query => query.Lat != null);
        RuleFor(query => query.Long)
            .NotEmpty()
            .When(query => query.Long != null);
        RuleFor(query => query.Phone)
            .NotEmpty()
            .When(query => query.Phone != null);
        RuleFor(query => query.Status)
            .NotEmpty()
            .When(query => query.Status != null);
        RuleFor(query => query.Role)
            .NotEmpty()
            .When(query => query.Role != null);
    }
}
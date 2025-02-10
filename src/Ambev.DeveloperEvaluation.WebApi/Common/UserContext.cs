using System.Security.Claims;
using Ambev.DeveloperEvaluation.Domain.Abstractions;
using Ambev.DeveloperEvaluation.Domain.Aggregates.Users.Enums;

namespace Ambev.DeveloperEvaluation.WebApi.Common;

internal sealed class UserContext : IUserContext
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserContext(IHttpContextAccessor httpContextAccessor)
    {
        ArgumentNullException.ThrowIfNull(httpContextAccessor);
        _httpContextAccessor = httpContextAccessor;
    }

    public bool IsAuthenticated => _httpContextAccessor.HttpContext?.User.Identity?.IsAuthenticated ?? false;

    public Guid? UserId => _httpContextAccessor.HttpContext?
        .User.Claims
        .FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier)?
        .Value.ToGuidOrDefault();

    public UserRole? UserRole => _httpContextAccessor.HttpContext?
        .User.Claims
        .FirstOrDefault(claim => claim.Type == ClaimTypes.Role)?
        .Value.ToEnumRoleOrDefault();
}

file static class StringExtensions
{
    public static Guid? ToGuidOrDefault(this string? str)
    {
        if(Guid.TryParse(str, out Guid guid)){
            return guid;
        }
        return null;
    }
    
    public static UserRole? ToEnumRoleOrDefault(this string? str)
    {
        if (str is null)
        {
            return null;
        }

        if (Enum.TryParse(str, out UserRole userRole))
        {
            return userRole;
        }

        return null;
    }
}
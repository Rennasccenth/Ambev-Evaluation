using System.Security.Claims;
using Ambev.DeveloperEvaluation.Common.Security;

namespace Ambev.DeveloperEvaluation.WebApi.Common;

internal sealed class UserContext : IUserContext
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserContext(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public bool IsAuthenticated => _httpContextAccessor.HttpContext?.User.Identity?.IsAuthenticated ?? false;

    public Guid? UserUuid => _httpContextAccessor.HttpContext?
        .User.Claims
        .FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier)?
        .Value.ToGuidOrDefault();
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
}
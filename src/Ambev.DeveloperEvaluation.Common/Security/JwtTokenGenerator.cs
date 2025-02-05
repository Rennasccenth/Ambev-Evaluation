using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;

namespace Ambev.DeveloperEvaluation.Common.Security;

public sealed class JwtTokenGenerator : IJwtTokenGenerator
{
    private readonly TimeProvider _timeProvider;
    private readonly JwtSettings _jwtSettings;

    public JwtTokenGenerator(
        IOptions<JwtSettings> jwtOptions,
        TimeProvider timeProvider)
    {
        _timeProvider = timeProvider;
        _jwtSettings = jwtOptions.Value;
    }

    public string GenerateToken(IUser user)
    {
        List<Claim> claims = 
        [
           new(ClaimTypes.NameIdentifier, user.Id),
           new(ClaimTypes.Name, user.Username),
           new(ClaimTypes.Role, user.Role),
           .. _jwtSettings.Audiences
               .Select(audience => new Claim(JwtRegisteredClaimNames.Aud, audience))
        ];

        SecurityTokenDescriptor tokenDescriptor = new()
        {
            Subject = new ClaimsIdentity(claims),
            Expires = _timeProvider.GetUtcNow()
                .AddMinutes(_jwtSettings.ExpirationMinutes).DateTime,
            SigningCredentials = new SigningCredentials(
                key: new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey)),
                algorithm: SecurityAlgorithms.HmacSha256Signature),
            Issuer = _jwtSettings.Issuer
        };

        JwtSecurityTokenHandler tokenHandler = new();
        JwtSecurityToken? token = tokenHandler.CreateJwtSecurityToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}
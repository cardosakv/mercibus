using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Auth.Application.Interfaces;
using Auth.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Auth.Infrastructure.Services;

public class JwtTokenService(IConfiguration configuration) : ITokenService
{
    public (string, long) CreateAccessToken(User user, string role)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var jwtKey = configuration["Jwt:Key"] ?? throw new InvalidOperationException("JWT secret key is not configured.");
        var key = Encoding.UTF8.GetBytes(jwtKey);

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id),
            new(ClaimTypes.Role, role),
            new(JwtRegisteredClaimNames.Sub, user.Id),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
        };

        var expireMillis = Convert.ToInt64(configuration["Jwt:ExpireMillis"]);
        var expireTime = DateTime.UtcNow.AddMilliseconds(expireMillis);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Issuer = configuration["Jwt:Issuer"] ?? string.Empty,
            Audience = configuration["Jwt:Audience"] ?? string.Empty,
            Expires = expireTime,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        var tokenString = tokenHandler.WriteToken(token);

        return (tokenString, expireMillis);
    }
}
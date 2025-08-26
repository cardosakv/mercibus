using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Auth.Application.Interfaces.Services;
using Auth.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Auth.Infrastructure.Services;

public class JwtTokenService(IConfiguration configuration) : ITokenService
{
    private readonly RsaSecurityKey _securityKey = GetRsaSecurityKey(configuration["Jwt:PrivateKeyPath"] ?? "jwt_priv_key.pem");

    public (string, long) CreateAccessToken(User user, string role)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id),
            new(ClaimTypes.Role, role),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
        };

        var expireSeconds = Convert.ToInt64(configuration["Jwt:ExpireSeconds"] ?? "300");
        var expireTime = DateTime.UtcNow.AddSeconds(expireSeconds);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Issuer = configuration["Jwt:Issuer"] ?? "mercibus-auth",
            Audience = configuration["Jwt:Audience"] ?? "mercibus-app",
            Expires = expireTime,
            SigningCredentials = new SigningCredentials(_securityKey, SecurityAlgorithms.RsaSha256)
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        var tokenString = tokenHandler.WriteToken(token);

        return (tokenString, expireSeconds);
    }

    private static RsaSecurityKey GetRsaSecurityKey(string privateKeyPath)
    {
        var rsa = RSA.Create();
        var text = File.ReadAllText(privateKeyPath);
        rsa.ImportFromPem(text);
        return new RsaSecurityKey(rsa);
    }
}
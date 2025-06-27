using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Auth.Application.DTOs;
using Auth.Application.Interfaces;
using Auth.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Auth.Application.Services
{
    public class TokenService(IConfiguration configuration) : ITokenService
    {
        public AuthToken CreateToken(User user, string role)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(configuration["Jwt:Key"] ?? string.Empty);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserName ?? string.Empty),
                new Claim(ClaimTypes.Email, user.Email ?? string.Empty),
                new Claim(ClaimTypes.Role, role),
            };
            
            var expireMillis = Convert.ToInt64(configuration["Jwt:ExpireMillis"]);
            var expireTime = DateTime.Now.AddMilliseconds(expireMillis);
            
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Issuer = configuration["Jwt:Issuer"] ?? string.Empty,
                Audience = configuration["Jwt:Audience"] ?? string.Empty,
                Expires = expireTime,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return new AuthToken
            {
                AccessToken = tokenString,
                ExpiresIn = expireMillis
            };
        }
    }
}

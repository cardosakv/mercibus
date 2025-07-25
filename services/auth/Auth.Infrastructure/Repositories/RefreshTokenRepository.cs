using Auth.Application.Common;
using Auth.Application.Interfaces.Repositories;
using Auth.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Auth.Infrastructure.Repositories;

public class RefreshTokenRepository(AppDbContext dbContext) : IRefreshTokenRepository
{
    public async Task<string> CreateTokenAsync(string userId)
    {
        var refreshTokenString = Utils.GenerateRandomString();

        var token = new RefreshToken
        {
            UserId = userId,
            ExpiresAt = DateTime.UtcNow.AddDays(7),
            TokenHash = Utils.HashString(refreshTokenString)
        };

        await dbContext.RefreshTokens.AddAsync(token);
        await dbContext.SaveChangesAsync();

        return refreshTokenString;
    }

    public async Task<RefreshToken?> RetrieveTokenAsync(string tokenString)
    {
        var hashedToken = Utils.HashString(tokenString);
        return await dbContext.RefreshTokens.FirstOrDefaultAsync(r => r.TokenHash == hashedToken);
    }

    public async Task<string> RotateTokenAsync(RefreshToken refreshToken)
    {
        refreshToken.IsRevoked = true;
        return await CreateTokenAsync(refreshToken.UserId);
    }

    public async Task<bool> RevokeTokenAsync(RefreshToken refreshToken)
    {
        refreshToken.IsRevoked = true;
        return await dbContext.SaveChangesAsync() > 0;
    }
}
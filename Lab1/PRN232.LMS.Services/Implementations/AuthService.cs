using Microsoft.EntityFrameworkCore;
using PRN232.LMS.Repositories.Entities;
using PRN232.LMS.Repositories.Repositories.Interfaces;
using PRN232.LMS.Services.Interfaces;
using PRN232.LMS.Services.Models;
using PRN232.LMS.Services.Results;
using PRN232.LMS.Services.Security;

namespace PRN232.LMS.Services.Implementations;

public sealed class AuthService : IAuthService
{
    private readonly IUserRepository _users;
    private readonly IRefreshTokenRepository _refreshTokens;
    private readonly IPasswordHashService _passwordHash;
    private readonly ITokenService _tokens;

    public AuthService(
        IUserRepository users,
        IRefreshTokenRepository refreshTokens,
        IPasswordHashService passwordHash,
        ITokenService tokens)
    {
        _users = users;
        _refreshTokens = refreshTokens;
        _passwordHash = passwordHash;
        _tokens = tokens;
    }

    public async Task<ServiceResult<AuthTokenModel>> LoginAsync(string username, string password)
    {
        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
        {
            return ServiceResult<AuthTokenModel>.Fail("Validation", "Username and password are required.");
        }

        var user = await _users.GetByUsernameAsync(username.Trim());
        if (user is null || !_passwordHash.Verify(user.PasswordHash, password))
        {
            return ServiceResult<AuthTokenModel>.Fail("Unauthorized", "Invalid username or password.");
        }

        var (accessToken, expiresIn) = _tokens.GenerateAccessToken(new UserTokenInfo(user.UserId, user.Username, user.Role));
        var refreshToken = _tokens.GenerateRefreshToken();
        var refreshTokenHash = _tokens.HashRefreshToken(refreshToken);

        await _refreshTokens.CreateAsync(new RefreshToken
        {
            UserId = user.UserId,
            TokenHash = refreshTokenHash,
            CreatedAt = DateTime.UtcNow,
            ExpiresAt = DateTime.UtcNow.AddDays(7),
            RevokedAt = null,
        });

        return ServiceResult<AuthTokenModel>.Ok(new AuthTokenModel(accessToken, refreshToken, expiresIn));
    }

    public async Task<ServiceResult<AuthTokenModel>> RefreshTokenAsync(string refreshToken)
    {
        if (string.IsNullOrWhiteSpace(refreshToken))
        {
            return ServiceResult<AuthTokenModel>.Fail("Validation", "RefreshToken is required.");
        }

        var tokenHash = _tokens.HashRefreshToken(refreshToken);
        var token = await _refreshTokens.QueryWithUser(asNoTracking: false)
            .FirstOrDefaultAsync(t => t.TokenHash == tokenHash);

        if (token is null || token.RevokedAt is not null || token.ExpiresAt <= DateTime.UtcNow || token.User is null)
        {
            return ServiceResult<AuthTokenModel>.Fail("Unauthorized", "Invalid refresh token.");
        }

        // Rotate refresh token
        token.RevokedAt = DateTime.UtcNow;
        await _refreshTokens.SaveChangesAsync();

        var (accessToken, expiresIn) = _tokens.GenerateAccessToken(new UserTokenInfo(token.User.UserId, token.User.Username, token.User.Role));
        var newRefreshToken = _tokens.GenerateRefreshToken();
        var newRefreshHash = _tokens.HashRefreshToken(newRefreshToken);

        await _refreshTokens.CreateAsync(new RefreshToken
        {
            UserId = token.User.UserId,
            TokenHash = newRefreshHash,
            CreatedAt = DateTime.UtcNow,
            ExpiresAt = DateTime.UtcNow.AddDays(7),
            RevokedAt = null,
        });

        return ServiceResult<AuthTokenModel>.Ok(new AuthTokenModel(accessToken, newRefreshToken, expiresIn));
    }
}

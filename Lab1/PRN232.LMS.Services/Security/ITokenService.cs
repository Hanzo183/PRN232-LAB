namespace PRN232.LMS.Services.Security;

public interface ITokenService
{
    (string Token, int ExpiresInSeconds) GenerateAccessToken(UserTokenInfo user);

    string GenerateRefreshToken();

    // Hashing refresh tokens allows safe DB storage.
    string HashRefreshToken(string refreshToken);
}

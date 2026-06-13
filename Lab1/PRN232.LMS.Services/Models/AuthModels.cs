namespace PRN232.LMS.Services.Models;

public sealed record AuthTokenModel(string AccessToken, string RefreshToken, int ExpiresIn);

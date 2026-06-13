namespace PRN232.LMS.API.Models.Responses;

public sealed record AuthTokenResponse(string AccessToken, string RefreshToken, int ExpiresIn);

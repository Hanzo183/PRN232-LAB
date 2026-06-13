using PRN232.LMS.Services.Models;
using PRN232.LMS.Services.Results;

namespace PRN232.LMS.Services.Interfaces;

public interface IAuthService
{
    Task<ServiceResult<AuthTokenModel>> LoginAsync(string username, string password);
    Task<ServiceResult<AuthTokenModel>> RefreshTokenAsync(string refreshToken);
}

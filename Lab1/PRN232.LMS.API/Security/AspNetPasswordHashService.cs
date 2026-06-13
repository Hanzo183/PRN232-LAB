using Microsoft.AspNetCore.Identity;
using PRN232.LMS.Repositories.Entities;
using PRN232.LMS.Services.Security;

namespace PRN232.LMS.API.Security;

public sealed class AspNetPasswordHashService : IPasswordHashService
{
    private readonly PasswordHasher<User> _hasher = new();

    public string Hash(string password)
        => _hasher.HashPassword(new User(), password);

    public bool Verify(string passwordHash, string password)
        => _hasher.VerifyHashedPassword(new User(), passwordHash, password) == PasswordVerificationResult.Success;
}

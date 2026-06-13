namespace PRN232.LMS.Services.Security;

public interface IPasswordHashService
{
    string Hash(string password);
    bool Verify(string passwordHash, string password);
}

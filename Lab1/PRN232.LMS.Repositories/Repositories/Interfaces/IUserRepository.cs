using PRN232.LMS.Repositories.Entities;

namespace PRN232.LMS.Repositories.Repositories.Interfaces;

public interface IUserRepository : IGenericRepository<User, int>
{
    Task<User?> GetByUsernameAsync(string username);
}

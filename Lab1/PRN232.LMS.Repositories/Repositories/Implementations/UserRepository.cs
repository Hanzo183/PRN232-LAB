using Microsoft.EntityFrameworkCore;
using PRN232.LMS.Repositories.Entities;
using PRN232.LMS.Repositories.Repositories.Interfaces;

namespace PRN232.LMS.Repositories.Repositories.Implementations;

public sealed class UserRepository : GenericRepository<User, int>, IUserRepository
{
    private readonly LmsDbContext _db;

    public UserRepository(LmsDbContext db)
        : base(db, nameof(User.UserId))
    {
        _db = db;
    }

    public Task<User?> GetByUsernameAsync(string username)
        => _db.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Username == username);
}

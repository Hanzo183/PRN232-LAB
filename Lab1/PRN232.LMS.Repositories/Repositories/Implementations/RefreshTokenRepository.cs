using Microsoft.EntityFrameworkCore;
using PRN232.LMS.Repositories.Entities;
using PRN232.LMS.Repositories.Repositories.Interfaces;

namespace PRN232.LMS.Repositories.Repositories.Implementations;

public sealed class RefreshTokenRepository : GenericRepository<RefreshToken, int>, IRefreshTokenRepository
{
    private readonly LmsDbContext _db;

    public RefreshTokenRepository(LmsDbContext db)
        : base(db, nameof(RefreshToken.RefreshTokenId))
    {
        _db = db;
    }

    public IQueryable<RefreshToken> QueryWithUser(bool asNoTracking = true)
    {
        var query = _db.RefreshTokens.Include(t => t.User);
        return asNoTracking ? query.AsNoTracking() : query;
    }
}

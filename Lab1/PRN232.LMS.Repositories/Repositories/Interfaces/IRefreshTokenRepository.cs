using PRN232.LMS.Repositories.Entities;

namespace PRN232.LMS.Repositories.Repositories.Interfaces;

public interface IRefreshTokenRepository : IGenericRepository<RefreshToken, int>
{
    IQueryable<RefreshToken> QueryWithUser(bool asNoTracking = true);
}

using Microsoft.EntityFrameworkCore;
using Revisio.Domain.Entities;

namespace Revisio.Application.Common.Interfaces
{
    public interface IAppDbContext
    {
        DbSet<RefreshToken> RefreshTokens { get; }
        DbSet<ApplicationUser> Users { get; }
        DbSet<Revisio.Domain.Entities.Course> Courses { get; }
        DbSet<Lectures> Lectures { get; }
        DbSet<PastExams> PastExams { get; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}

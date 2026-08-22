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
        DbSet<GenerationRequest> GenerationRequests { get; }
        DbSet<GenerationRequestLecture>GenerationRequestLectures { get; } 
        DbSet<Revisio.Domain.Entities.Questions> Questions { get; }
        DbSet<QuestionOptions> QuestionOptions { get; }
        DbSet<ExamSession> ExamSessions { get; }
        DbSet<ExamSessionAnswer> ExamSessionAnswers { get; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        Task<int> ExecuteSqlRawAsync(string sql, params object[] parameters);
        Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry Entry(object entity);
    }
}

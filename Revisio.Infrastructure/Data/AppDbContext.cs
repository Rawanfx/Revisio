using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Revisio.Application.Common.Interfaces;
using Revisio.Domain.Entities;
using System.Reflection.Emit;
using MassTransit.EntityFrameworkCoreIntegration;
using MassTransit;

namespace Revisio.Infrastructure.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>,IAppDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) :base(options) {}
        public DbSet<ApplicationUser> Users => Set<ApplicationUser>();
        public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
        public DbSet<PastExams> PastExams => Set<PastExams>();
        public DbSet<Course> Courses => Set<Course>();
        public DbSet<GenerationRequest> GenerationRequests => Set<GenerationRequest>();
        public DbSet<Lectures> Lectures => Set<Lectures>();
        public DbSet<Questions> Questions => Set<Questions>();
        public DbSet<QuestionOptions> QuestionOptions => Set<QuestionOptions>();
        public DbSet<GenerationRequestLecture> GenerationRequestLectures => Set<GenerationRequestLecture>();
        public DbSet<ExamSession> ExamSessions => Set<ExamSession>();
        public DbSet<ExamSessionAnswer> ExamSessionAnswers => Set<ExamSessionAnswer>();
        public async Task<int> ExecuteSqlRawAsync(string sql, params object[] parameters)
        {
            return await Database.ExecuteSqlRawAsync(sql, parameters);
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
            builder.AddOutboxMessageEntity();
            builder.AddOutboxStateEntity();
            builder.Entity<GenerationRequestLecture>()
                .HasKey(x => new { x.LectureId, x.GenerationRequestId });
            
        }
    }
}

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Revisio.Application.Common.Interfaces;
using Revisio.Domain.Entities;

namespace Revisio.Infrastructure.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>,IAppDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) :base(options) {}
        public DbSet<ApplicationUser> Users => Set<ApplicationUser>();
        public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
        public DbSet<PastExams> PastExams => Set<PastExams>();
        public DbSet<Course> Courses => Set<Course>();
        public DbSet<Lectures> Lectures => Set<Lectures>();
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        }
    }
}

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
    }
}

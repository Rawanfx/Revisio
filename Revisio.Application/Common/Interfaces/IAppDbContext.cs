using Microsoft.EntityFrameworkCore;
using Revisio.Domain.Entities;

namespace Revisio.Application.Common.Interfaces
{
    public interface IAppDbContext
    {
        DbSet<ApplicationUser> Users { get; }
    }
}

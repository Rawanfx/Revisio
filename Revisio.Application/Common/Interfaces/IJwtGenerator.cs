using Revisio.Domain.Entities;

namespace Revisio.Application.Common.Interfaces
{
    public interface IJwtGenerator
    {
        string GenerateJwtToken(ApplicationUser user);
    }
}

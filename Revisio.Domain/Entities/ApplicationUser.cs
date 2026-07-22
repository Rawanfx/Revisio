using Microsoft.AspNetCore.Identity;

namespace Revisio.Domain.Entities
{
    public class ApplicationUser:IdentityUser
    {
        public string FullName { get; set; }
        public string? RefreshToken { get; set; }
        public UserRole Role { get; set; }
    }
}

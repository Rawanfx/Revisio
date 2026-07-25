using Revisio.Application.Common.Interfaces;
using System.Security.Claims;

namespace Revisio.API.Service
{
    public class CurrentUser : ICurrentUserService
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        public CurrentUser(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }
        public string UserId { get => httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier); }
    }
}

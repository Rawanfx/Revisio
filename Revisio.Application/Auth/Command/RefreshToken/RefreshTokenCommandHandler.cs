using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Revisio.Application.Auth.Common;
using Revisio.Application.Common.Exceptions;
using Revisio.Application.Common.Interfaces;
using Revisio.Application.Common.Models;
using Revisio.Domain.Entities;

namespace Revisio.Application.Auth.Command.RefreshTokens
{
    public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, Response<LoginResponseDto>>
    {
        private readonly IAppDbContext context;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IJwtGenerator jwtGenerator;
        public RefreshTokenCommandHandler(IAppDbContext context
            , UserManager<ApplicationUser> userManager
            , IJwtGenerator jwtGenerator)
        {
            this.userManager = userManager;
            this.jwtGenerator = jwtGenerator;
            this.context = context;
        }
        public async Task<Response<LoginResponseDto>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            var storedToken = await context.RefreshTokens
               .FirstOrDefaultAsync(t => t.Token == request.Token, cancellationToken);

            if (storedToken is null || storedToken.IsRevoked || storedToken.ExpiresAt < DateTime.UtcNow)
                throw new UnauthorizedException("Invalid or expired refresh token");
            var user = await userManager.FindByIdAsync(storedToken.UserId);
            if (user is null)
                throw new UnauthorizedException("Invalid refresh token");
            storedToken.IsRevoked = true;
            var newAccessToken = jwtGenerator.GenerateJwtToken(user);
            var newRefreshToken = jwtGenerator.GenerateRefreshToken();

            context.RefreshTokens.Add(new RefreshToken
            {
                Id = Guid.NewGuid(),
                Token = newRefreshToken,
                UserId = user.Id,
                ExpiresAt = DateTime.UtcNow.AddDays(7),
                CreatedAt = DateTime.UtcNow,
                IsRevoked = false
            });
            await context.SaveChangesAsync(cancellationToken);

            return new Response<LoginResponseDto>
            {
                Success = true,
                Data = new LoginResponseDto
                {
                    UserId = user.Id,
                    Token = newAccessToken,
                    RefreshToken = newRefreshToken
                }
            };
        }
           
    }
}

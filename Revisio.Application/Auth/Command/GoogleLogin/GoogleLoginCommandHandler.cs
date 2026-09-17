using MediatR;
using Microsoft.AspNetCore.Identity;
using Revisio.Application.Auth.Common;
using Revisio.Application.Common.Interfaces;
using Revisio.Application.Common.Models;
using Revisio.Domain.Entities;

namespace Revisio.Application.Auth.Command.GoogleLogin
{
    public class GoogleLoginCommandHandler : IRequestHandler<GoogleLoginCommand, Response<LoginResponseDto>>
    {
        private readonly IAppDbContext context;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IJwtGenerator jwtGenerator;
            public GoogleLoginCommandHandler(IAppDbContext context, UserManager<ApplicationUser> userManager, IJwtGenerator jwtGenerator)
        {
            this.context = context;
            this.userManager = userManager;
            this.jwtGenerator = jwtGenerator;
        }

        public async Task<Response<LoginResponseDto>> Handle(GoogleLoginCommand request, CancellationToken cancellationToken)
        {
            var existingUser = await userManager.FindByEmailAsync(request.Email);

            if (existingUser is null)
            {
                existingUser = new ApplicationUser
                {
                    Email = request.Email,
                    UserName = request.Email,
                };
                await userManager.CreateAsync(existingUser);
            }
            var accessToken = jwtGenerator.GenerateJwtToken(existingUser);

            var refreshToken = new RefreshToken
            {
                Id = Guid.NewGuid(),
                UserId = existingUser.Id,
                Token = jwtGenerator.GenerateRefreshToken(),  
                ExpiresAt = DateTime.UtcNow.AddDays(7),     
                CreatedAt = DateTime.UtcNow
            };

            context.RefreshTokens.Add(refreshToken);
            await context.SaveChangesAsync(cancellationToken);
            return new Response<LoginResponseDto>
            {
                Success = true,
                Data = new LoginResponseDto
                {
                    Token = accessToken,
                    RefreshToken = refreshToken.Token
                }
            };
        }
    }
}

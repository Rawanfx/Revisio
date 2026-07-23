using MediatR;
using Microsoft.AspNetCore.Identity;
using Revisio.Application.Auth.Common;
using Revisio.Application.Common.Exceptions;
using Revisio.Application.Common.Interfaces;
using Revisio.Application.Common.Models;
using Revisio.Domain.Entities;

namespace Revisio.Application.Auth.Command.Login
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, Response<LoginResponseDto>>
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IJwtGenerator jwtGenerator;
        private readonly IAppDbContext context;
        public LoginCommandHandler(UserManager<ApplicationUser> userManager
            ,IJwtGenerator jwtGenerator
            ,IAppDbContext context)
        {
            this.userManager = userManager;
            this.jwtGenerator = jwtGenerator;
            this.context = context;
        }
        public async Task<Response<LoginResponseDto>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = await userManager.FindByEmailAsync(request.Email);
           
            if (user == null || !await userManager.CheckPasswordAsync(user,request.Password))
                throw new UnauthorizedException("Invalid Email or Password");

            if (!user.EmailConfirmed)
                throw new UnauthorizedException("Please confirm your email before logging in");

            string token = jwtGenerator.GenerateJwtToken(user);
            var refreshToken = jwtGenerator.GenerateRefreshToken();

            context.RefreshTokens.Add(new RefreshToken
            {
                Id = Guid.NewGuid(),
                Token = refreshToken,
                UserId = user.Id,
                ExpiresAt = DateTime.UtcNow.AddDays(7),
                CreatedAt = DateTime.UtcNow,
                IsRevoked = false
            });
            await context.SaveChangesAsync(cancellationToken);
            return new Response<LoginResponseDto>()
            {
                Success = true,
                Data = new LoginResponseDto()
                {
                    UserId = user.Id,
                    Token = token,
                    RefreshToken=refreshToken
                }
            };
        }

     
    }
}

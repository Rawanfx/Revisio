using MediatR;
using Microsoft.AspNetCore.Identity;
using Revisio.Application.Auth.Common;
using Revisio.Application.Common.Exceptions;
using Revisio.Application.Common.Interfaces;
using Revisio.Application.Common.Models;
using Revisio.Domain.Entities;

namespace Revisio.Application.Auth.Command.Login
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, Response<RegisterResponseDto>>
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IJwtGenerator jwtGenerator;
        public LoginCommandHandler(UserManager<ApplicationUser> userManager
            ,IJwtGenerator jwtGenerator)
        {
            this.userManager = userManager;
            this.jwtGenerator = jwtGenerator;
        }
        public async Task<Response<RegisterResponseDto>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = await userManager.FindByEmailAsync(request.Email);
           
            if (user == null || !await userManager.CheckPasswordAsync(user,request.Password))
                throw new UnauthorizedException("Invalid Email or Password");

            if (!user.EmailConfirmed)
                throw new UnauthorizedException("Please confirm your email before logging in");

            string token = jwtGenerator.GenerateJwtToken(user);

            return new Response<RegisterResponseDto>()
            {
                Success = true,
                Data = new RegisterResponseDto()
                {
                    UserId = user.Id,
                    Token = token
                }
            };
        }
    }
}

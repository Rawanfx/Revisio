using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Revisio.Application.Auth.Common;
using Revisio.Application.Common.Exceptions;
using Revisio.Application.Common.Interfaces;
using Revisio.Application.Common.Models;
using Revisio.Domain.Entities;

namespace Revisio.Application.Auth.Command.Register
{
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, Response<RegisterResponseDto>>
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IMailService mailService;
        private readonly IJwtGenerator jwtGenerator;
        public RegisterCommandHandler (UserManager<ApplicationUser> userManager
            ,IJwtGenerator jwtGenerator
            ,IMailService mail)
        {
            this.userManager = userManager;
            this.jwtGenerator = jwtGenerator;
            this.mailService = mail;
        }
       
        public async Task<Response<RegisterResponseDto>> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            var user = new ApplicationUser()
            {
                Email=request.Email,
                FullName= request.FullName,
                UserName = request.Email,
                Role = request.UserRole
            };
            var result = await userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded)
                throw new IdentityException(result.Errors.Select(x=>x.Description).ToList());
            
            var confirmationToken = await userManager.GenerateEmailConfirmationTokenAsync(user);
            var encodedToken = Uri.EscapeDataString(confirmationToken);
            
            //send confirmation token to email
            await mailService.SendConfirmationEmail ( user.Email,encodedToken);
           
            return new Response<RegisterResponseDto>() {Success=true,Message ="Check your email" };
        }
    }
}

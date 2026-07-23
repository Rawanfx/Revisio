using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Revisio.Application.Auth.Common;
using Revisio.Application.Common.Exceptions;
using Revisio.Application.Common.Interfaces;
using Revisio.Application.Common.Models;
using Revisio.Domain.Entities;

namespace Revisio.Application.Auth.Command.ResendConfirmEmail
{
    public class ResendConfirmEmailCommandHandler : IRequestHandler<ResendConfirmEmailCommand, Response<string>>
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IMailService mailService;
        private readonly IConfiguration configuration;
        public ResendConfirmEmailCommandHandler(UserManager<ApplicationUser> userManager
           ,IMailService mailService
            ,IConfiguration configuration)
        {
            this.userManager = userManager;
            this.mailService = mailService;
            this.configuration = configuration;
        }
        public async Task<Response<string>> Handle(ResendConfirmEmailCommand request, CancellationToken cancellationToken)
        {
            //check user is found
            var user = await userManager.FindByEmailAsync(request.Email);
            if (user != null)
            {
                var confirmationToken = await userManager.GenerateEmailConfirmationTokenAsync(user);
                var encodedToken = Uri.EscapeDataString(confirmationToken);
              
                await mailService.SendConfirmationEmail(  user.Email,encodedToken);
            }
            return new Response<string>() { Success = true, Message = "If this email is registered, a confirmation link has been sent" };
        }
    }
}

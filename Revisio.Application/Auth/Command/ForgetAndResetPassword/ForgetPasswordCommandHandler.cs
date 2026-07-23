using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Revisio.Application.Common.Exceptions;
using Revisio.Application.Common.Interfaces;
using Revisio.Application.Common.Models;
using Revisio.Domain.Entities;
using System.Text;

namespace Revisio.Application.Auth.Command.ForgetAndResetPassword
{
    public class ForgetPasswordCommandHandler : IRequestHandler<ForgetPasswordCommand, Response<string>>
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IMailService mailService;
        public ForgetPasswordCommandHandler(UserManager<ApplicationUser> userManager
            ,IMailService mailService)
        {
            this.userManager = userManager;
            this.mailService = mailService;
        }
        public async Task<Response<string>> Handle(ForgetPasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await userManager.FindByEmailAsync(request.Email);
            if (user != null && user.EmailConfirmed)
            {
                var token = await userManager.GeneratePasswordResetTokenAsync(user);
                var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
                await mailService.SendForgetPasswordEmail(request.Email, encodedToken, request.ClientUri);
            }

            return new Response<string>
            {
                Success = true,
                Message = "If this email is registered, you'll receive a password reset link"
            };
        }
    }
}

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
    public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand, Response<string>>
    {
        private readonly UserManager<ApplicationUser> userManager;
        public ResetPasswordCommandHandler(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }
        public async Task<Response<string>> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await userManager.FindByEmailAsync(request.Email);
            if (user == null || !user.EmailConfirmed)
                throw new NotFoundException("Invalid Email");
            var decodedToken =Encoding.UTF8.GetString( WebEncoders.Base64UrlDecode(request.token));
            var changePasswordResult = await userManager.ResetPasswordAsync(user, decodedToken, request.NewPassword);
            if (!changePasswordResult.Succeeded)
                throw new IdentityException(changePasswordResult.Errors.Select(x => x.Description).ToList());
            return new Response<string>() { Success = true,Message="Password changed succesfully" };
        }
    }
}

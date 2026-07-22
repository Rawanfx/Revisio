using MediatR;
using Microsoft.AspNetCore.Identity;
using Revisio.Application.Common.Exceptions;
using Revisio.Application.Common.Models;
using Revisio.Domain.Entities;

namespace Revisio.Application.Auth.Command.ConfirmEmail
{
    public class ConfirmEmailCommandHandler : IRequestHandler<ConfirmEmailCommand, Response<string>>
    {
        private readonly UserManager<ApplicationUser> userManager;
        public ConfirmEmailCommandHandler (UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }
        public async Task<Response<string>> Handle(ConfirmEmailCommand request, CancellationToken cancellationToken)
        {
            var user = await userManager.FindByEmailAsync(request.Email);
            if (user == null)
                throw new NotFoundException("User not found");
            var decodedToken = Uri.UnescapeDataString(request.Token);
            var result = await userManager.ConfirmEmailAsync(user, decodedToken);

            if (!result.Succeeded)
                throw new IdentityException(result.Errors.Select(e => e.Description).ToList());
            return new Response<string>
            {
                Success = true,
                Message = "Email Confirmed"
            };
        }
    }
}

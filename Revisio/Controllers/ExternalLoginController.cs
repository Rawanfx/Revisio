
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Mvc;
using Revisio.Application.Auth.Command.GoogleLogin;
using System.Security.Claims;

namespace Revisio.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExternalLoginController:ControllerBase
    {
        private readonly IMediator _mediator;
        public ExternalLoginController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpGet]
        public IActionResult test()
        {
            return Ok();
        }
        [HttpGet("google-response")]
        public async Task<IActionResult> GoogleCallback()
        {
            var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            if (!result.Succeeded || result.Principal == null)
            {
                return BadRequest("Google authentication failed");
            }

            // استخراج الإيميل سواء بالاسم القياسي أو من الـ claim الخام لجوجل
            var email = result.Principal.FindFirstValue(ClaimTypes.Email)
                     ?? result.Principal.FindFirstValue("email");

            var name = result.Principal.FindFirstValue(ClaimTypes.Name)
                    ?? result.Principal.FindFirstValue("name");

            if (string.IsNullOrEmpty(email))
            {
                return BadRequest("Email not received from Google");
            }

            var command = new GoogleLoginCommand(email, name ?? "User");
            var response = await _mediator.Send(command);

            return Ok(response);
        }
        [HttpGet("login-google")]
        public IActionResult LoginWithGoogle()
        {
            var redirectUrl = Url.Action(nameof(GoogleCallback));
            var properties = new AuthenticationProperties { RedirectUri = redirectUrl };
            return Challenge(properties, GoogleDefaults.AuthenticationScheme);
        }

    }
}

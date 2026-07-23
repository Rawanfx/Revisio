using MediatR;
using Microsoft.AspNetCore.Mvc;
using Revisio.Application.Auth.Command.ConfirmEmail;
using Revisio.Application.Auth.Command.ForgetAndResetPassword;
using Revisio.Application.Auth.Command.Login;
using Revisio.Application.Auth.Command.Register;
using Revisio.Application.Auth.Command.ResendConfirmEmail;

namespace Revisio.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;
        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterCommand command)
        {
            var response = await _mediator.Send(command);
            return Ok(response);
        }
        [HttpGet("confirm-email")]
        public async Task<IActionResult> ConfirmEmail([FromQuery] ConfirmEmailCommand command)
        {
            var response = await _mediator.Send(command);
            return Ok(response);
        }
        [HttpGet("resend-confirm-email")]
        public async Task<IActionResult> ResendConfirmEmail([FromQuery] ResendConfirmEmailCommand command)
        {
            var response = await _mediator.Send(command);
            return Ok(response);
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginCommand command)
        {
            var response = await _mediator.Send(command);
            return Ok(response);
        }
        [HttpPost("forget-password")]
        public async Task<IActionResult>ForgetPassword([FromBody]ForgetPasswordCommand command)
        {
            var response = await _mediator.Send(command);
            return Ok(response);
        }
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordCommand command)
        {
            var response = await _mediator.Send(command);
            return Ok(response);
        }
    }
}

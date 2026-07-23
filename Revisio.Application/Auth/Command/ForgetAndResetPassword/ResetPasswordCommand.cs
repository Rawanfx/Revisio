using MediatR;
using Revisio.Application.Common.Models;

namespace Revisio.Application.Auth.Command.ForgetAndResetPassword;

public record ResetPasswordCommand(string Email,string token,string NewPassword):IRequest<Response<string>>;


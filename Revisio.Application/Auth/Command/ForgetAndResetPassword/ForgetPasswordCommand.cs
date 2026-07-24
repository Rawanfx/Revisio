
using MediatR;
using Revisio.Application.Common.Models;

namespace Revisio.Application.Auth.Command.ForgetAndResetPassword;

public record ForgetPasswordCommand(string Email):IRequest<Response<string>>;


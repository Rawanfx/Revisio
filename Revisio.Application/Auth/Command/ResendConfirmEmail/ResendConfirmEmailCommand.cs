using MediatR;
using Revisio.Application.Common.Models;

namespace Revisio.Application.Auth.Command.ResendConfirmEmail;
    public record ResendConfirmEmailCommand(string Email):IRequest<Response<string>>;
   


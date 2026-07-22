using MediatR;
using Revisio.Application.Common.Models;
namespace Revisio.Application.Auth.Command.ConfirmEmail
{
    public record ConfirmEmailCommand(string Email,string Token):IRequest <Response<string>>;
}

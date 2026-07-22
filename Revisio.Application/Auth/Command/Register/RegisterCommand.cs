using MediatR;
using Revisio.Application.Auth.Common;
using Revisio.Application.Common.Models;
using Revisio.Domain.Entities;

namespace Revisio.Application.Auth.Command.Register
{
    public record RegisterCommand(string FullName,string Email,string Password,UserRole UserRole)
        :IRequest<Response<RegisterResponseDto>>;
   
}

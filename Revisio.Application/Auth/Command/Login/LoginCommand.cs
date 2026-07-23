using MediatR;
using Revisio.Application.Auth.Common;
using Revisio.Application.Common.Models;

namespace Revisio.Application.Auth.Command.Login;

public record LoginCommand(string Email,string Password):IRequest<Response<LoginResponseDto>>;

using Revisio.Application.Common.Models;
using MediatR;
using Revisio.Application.Auth.Common;

namespace Revisio.Application.Auth.Command.GoogleLogin;

public record GoogleLoginCommand(string Email, string Name) : IRequest<Response<LoginResponseDto>>;


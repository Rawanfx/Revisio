using MediatR;
using Revisio.Application.Auth.Common;
using Revisio.Application.Common.Models;

namespace Revisio.Application.Auth.Command.RefreshTokens;

public record RefreshTokenCommand(string Token):IRequest<Response<LoginResponseDto>>;


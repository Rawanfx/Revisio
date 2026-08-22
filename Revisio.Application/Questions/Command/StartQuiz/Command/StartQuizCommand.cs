using MediatR;
using Revisio.Application.Common.Models;
using Revisio.Application.Questions.Dto;

namespace Revisio.Application.Questions.Command.StartQuiz.Command;

public record StartQuizCommand(Guid GenerationRequestId):IRequest<Response<StartQuizResponse>>;


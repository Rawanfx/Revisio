using MediatR;
using Revisio.Application.Common.Models;
using Revisio.Application.Questions.Dto;

namespace Revisio.Application.Questions.Command.StartQuiz;

public record StartQuizCommand(Guid GenerationRequestId):IRequest<Response<StartQuizResponse>>;


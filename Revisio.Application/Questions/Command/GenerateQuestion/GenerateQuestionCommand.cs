using MediatR;
using Revisio.Application.Common.Models;
using Revisio.Application.Questions.Dto;

namespace Revisio.Application.Questions.Command.GenerateQuestion;

public record GenerateQuestionCommand(GenerateQuestionRequestDto GenerateQuestionRequestDto)
    :IRequest<Response<Guid>>;


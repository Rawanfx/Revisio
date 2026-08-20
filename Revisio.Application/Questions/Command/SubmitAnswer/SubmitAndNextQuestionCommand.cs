
using Revisio.Application.Common.Models;
using MediatR;
using Microsoft.AspNetCore.Http;
using Revisio.Application.Questions.Dto;
using Revisio.Domain.Enums;

namespace Revisio.Application.Questions.Command.SubmitAnswer;

public record SubmitAndNextQuestionCommand (
    Guid GenerationRequestId ,
    Guid StartQuizId,
    Guid QuestionId,
    string? EssayAnswer,
    Guid? McqAnswer,
    TimeSpan TimeTakeToAnswer
    ):IRequest<Response<SubmitAndNextQuestionResponse>>;


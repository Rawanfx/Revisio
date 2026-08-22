using Revisio.Application.Common.Models;
using MediatR;
using Revisio.Application.Questions.Dto;

namespace Revisio.Application.Questions.Query.ExamResult;

public record ExamResultQuery(Guid ExamSessionId):IRequest<Response<ExamResultDto>>;

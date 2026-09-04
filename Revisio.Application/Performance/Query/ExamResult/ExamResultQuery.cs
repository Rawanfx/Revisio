using Revisio.Application.Common.Models;
using MediatR;
using Revisio.Application.Performance.Dto;

namespace Revisio.Application.Performance.Query.ExamResult;

public record ExamResultQuery(Guid ExamSessionId):IRequest<Response<ExamResultDto>>;

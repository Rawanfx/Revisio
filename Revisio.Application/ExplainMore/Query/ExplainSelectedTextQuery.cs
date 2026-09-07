using MediatR;
using Revisio.Application.ExplainMore.Dto;
using Revisio.Application.Common.Models;
namespace Revisio.Application.ExplainMore.Query;

public record ExplainSelectedTextQuery(Guid QuestionId, Guid ExamSessionId,string SelectedConcept) : IRequest<Response<ExplainTextResponse>>;


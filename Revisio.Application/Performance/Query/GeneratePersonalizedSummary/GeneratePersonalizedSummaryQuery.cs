using MediatR;
using Revisio.Application.Common.Models;
using Revisio.Application.Performance.Dto;
namespace Revisio.Application.Performance.Query.GeneratePersonalizedSummary;

public record GeneratePersonalizedSummaryQuery(Guid CourseId) : IRequest<Response<GeneatePreExamSummaryDto>>;

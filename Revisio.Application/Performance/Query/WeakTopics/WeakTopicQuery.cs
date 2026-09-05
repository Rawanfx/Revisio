using MediatR;
using Revisio.Application.Common.Models;
using Revisio.Application.Performance.Dto;
namespace Revisio.Application.Performance.Query.WeakTopics;

public record WeakTopicQuery(Guid CourseId) : IRequest<Response<WeakReviewDto>>;
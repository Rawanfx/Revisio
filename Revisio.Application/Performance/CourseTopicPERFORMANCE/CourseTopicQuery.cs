using MediatR;
using Revisio.Application.Common.Models;
using Revisio.Application.Performance.Dto;
namespace Revisio.Application.Performance.CourseTopicPERFORMANCE;

public record CourseTopicQuery(Guid CourseId) : IRequest<Response<CourseTopicDto>>;        


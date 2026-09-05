using MediatR;
using Revisio.Application.Common.Models;
using Revisio.Application.Performance.Dto;
namespace Revisio.Application.Performance.Query.Attendence;

public record AttendenceQuery(Guid CourseId) : IRequest<Response<AttendenceDto>>;

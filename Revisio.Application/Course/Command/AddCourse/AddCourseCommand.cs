using MediatR;
using Revisio.Application.Common.Models;
using Revisio.Domain.Enums;

namespace Revisio.Application.Course.Command.AddCourse;

public record AddCourseCommand(string CourseName,string InstructorName,Semesters Semesters)
    : IRequest<Response<Guid>>;


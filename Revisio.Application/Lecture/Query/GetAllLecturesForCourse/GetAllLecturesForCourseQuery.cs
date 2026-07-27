using MediatR;
using Revisio.Application.Common.Models;
using Revisio.Application.Course.Common.Models;
using Revisio.Application.Lecture.Common;

namespace Revisio.Application.Lecture.Query.GetAllLecturesForCourse;

public record GetAllLecturesForCourseQuery(Guid CourseId):IRequest<Response<List<AllLectureDto>>>;


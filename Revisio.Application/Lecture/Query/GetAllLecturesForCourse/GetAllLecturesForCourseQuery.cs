using MediatR;
using Revisio.Application.Common.Models;
using Revisio.Application.Course.Common.Models;
using Revisio.Application.Lecture.Common;

namespace Revisio.Application.Lecture.Query.GetAllLecturesForCourse;

public record GetAllLecturesForCourseQuery(Guid CourseId, int PageNum = 1, int PageSize = 10)
    : IRequest<Response<PaginatedList<AllLectureDto>>>;


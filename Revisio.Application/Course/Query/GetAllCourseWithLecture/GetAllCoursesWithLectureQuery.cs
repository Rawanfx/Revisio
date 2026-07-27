using MediatR;
using Revisio.Application.Common.Models;
using Revisio.Application.Course.Common.Dtos;
using Revisio.Application.Course.Common.Models;

namespace Revisio.Application.Course.Query.GetAllCourseWithLecture;

public record GetAllCoursesWithLectureQuery(int pageNum,int pageSize)
    :IRequest<Response<PaginatedList<CourseDto>>>;

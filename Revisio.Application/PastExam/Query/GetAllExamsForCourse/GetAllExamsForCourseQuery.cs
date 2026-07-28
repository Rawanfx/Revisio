using MediatR;
using Revisio.Application.Common.Models;
using Revisio.Application.Course.Common.Models;
using Revisio.Application.PastExam.Common;

namespace Revisio.Application.PastExam.Query.GetAllExamsForCourse;

public record GetAllExamsForCourseQuery(Guid CourseId,int pageNum,int pageSize):IRequest<Response<PaginatedList<PastExamDto>>>;


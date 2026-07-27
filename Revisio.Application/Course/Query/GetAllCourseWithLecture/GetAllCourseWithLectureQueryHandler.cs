using MediatR;
using Microsoft.EntityFrameworkCore;
using Revisio.Application.Common.Exceptions;
using Revisio.Application.Common.Interfaces;
using Revisio.Application.Common.Models;
using Revisio.Application.Course.Common.Dtos;
using Revisio.Application.Course.Common.Models;
using Revisio.Domain.Entities;

namespace Revisio.Application.Course.Query.GetAllCourseWithLecture
{
    public class GetAllCourseWithLectureQueryHandler : IRequestHandler<GetAllCoursesWithLectureQuery, Response<PaginatedList<CourseDto>>>
    {
        private readonly ICurrentUserService currentUser;
        private readonly IAppDbContext context;
        public GetAllCourseWithLectureQueryHandler(ICurrentUserService currentUser, IAppDbContext context)
        {
            this.currentUser = currentUser;
            this.context = context;
        }

        public async Task<Response<PaginatedList<CourseDto>>> Handle(GetAllCoursesWithLectureQuery request, CancellationToken cancellationToken)
        {
            var user = currentUser.UserId;
            if (user == null)
                throw new UnauthorizedException("user not found");

            var query = context.Courses
                .Where(x => x.UserId == user)
                .Select(x => new CourseDto()
                {
                    CourseId =x.Id,
                   LecNum= x.Lectures.Count,
                   ExamNum=x.PastExams.Count,
                   CourseName=x.CourseName,
                   ProfName=x.InstructorName
                });
            var res = await PaginatedList<CourseDto>.CreateAsync(
                query, request.pageNum, request.pageSize);
            return new Response<PaginatedList<CourseDto>>
            {
                Data = res,
                Success = true
            };
        }
    }
}

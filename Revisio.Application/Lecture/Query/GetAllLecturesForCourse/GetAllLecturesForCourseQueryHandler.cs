using MediatR;
using Microsoft.EntityFrameworkCore;
using Revisio.Application.Common.Exceptions;
using Revisio.Application.Common.Interfaces;
using Revisio.Application.Common.Models;
using Revisio.Application.Course.Common.Models;
using Revisio.Application.Lecture.Common;
using System.Collections.Generic;

namespace Revisio.Application.Lecture.Query.GetAllLecturesForCourse
{
    public class GetAllLecturesForCourseQueryHandler : IRequestHandler<GetAllLecturesForCourseQuery, Response<PaginatedList<AllLectureDto>>>
    {
        private readonly IAppDbContext context;
        private readonly ICurrentUserService currentUser;
        private readonly IUploadToCloud uploadToCloud;
        public GetAllLecturesForCourseQueryHandler(IAppDbContext context
            , ICurrentUserService currentUser
            , IUploadToCloud uploadToCloud)
        {
            this.context = context;
            this.currentUser = currentUser;
            this.uploadToCloud = uploadToCloud;
        }
        public async Task<Response<PaginatedList<AllLectureDto>>> Handle(
    GetAllLecturesForCourseQuery request, CancellationToken cancellationToken)
        {
            var user = currentUser.UserId;
            if (user == null)
                throw new UnauthorizedException("Unauthorized user");

            var course = await context.Courses
                .FirstOrDefaultAsync(x => x.UserId == user && x.Id == request.CourseId);
            if (course == null)
                throw new NotFoundException("Course not found");

            var query = context.Lectures
                .Where(x => x.CourseId == request.CourseId)
                .Select(x => new AllLectureDto()
                {
                    LecName = x.LecName,
                    UploadDate = x.UploadedAt,
                    UploadUrl = x.UploadUrl
                });

            var result = await PaginatedList<AllLectureDto>.CreateAsync(
                query, request.PageNum, request.PageSize);

            foreach (var i in result.Items)
            {
                i.UploadUrl = await uploadToCloud.GenerateUrl(i.UploadUrl);
            }

            return new Response<PaginatedList<AllLectureDto>>()
            {
                Data = result,
                Success = true
            };
        }
    }
}

using MediatR;
using Microsoft.EntityFrameworkCore;
using Revisio.Application.Common.Exceptions;
using Revisio.Application.Common.Interfaces;
using Revisio.Application.Common.Models;
using Revisio.Application.Course.Common.Models;
using Revisio.Application.PastExam.Common;

namespace Revisio.Application.PastExam.Query.GetAllExamsForCourse
{
    public class GetAllExamsForCourseQueryHandler
        : IRequestHandler<GetAllExamsForCourseQuery, Response<PaginatedList<PastExamDto>>>
    {
        private readonly IAppDbContext context;
        private readonly ICurrentUserService currentUser;
        private readonly IUploadToCloud uploadToCloud;
        public GetAllExamsForCourseQueryHandler(IAppDbContext context
           , ICurrentUserService currentUser
           , IUploadToCloud uploadToCloud)
        {
            this.context = context;
            this.currentUser = currentUser;
            this.uploadToCloud = uploadToCloud;
        }
        public async Task<Response<PaginatedList<PastExamDto>>> Handle(GetAllExamsForCourseQuery request, CancellationToken cancellationToken)
        {
            var user = currentUser.UserId;
            if (user == null)
                throw new UnauthorizedException("Unauthorized user");

            var course = await context.Courses
                .FirstOrDefaultAsync(x => x.UserId == user && x.Id == request.CourseId);
            if (course == null)
                throw new NotFoundException("Course not found");

            var query = context.PastExams
                .Where(x => x.CourseId == request.CourseId)
                .Select(x => new PastExamDto()
                {
                   UploadUrl=x.UploadUrl,
                   ExamType=x.ExamType.ToString(),
                   profName = x.InstructorName,
                   UploadDate=x.UploadDate,
                });

            var result = await PaginatedList<PastExamDto>.CreateAsync(
                query, request.pageNum, request.pageSize);

            foreach (var i in result.Items)
            {
                i.UploadUrl = await uploadToCloud.GenerateUrl(i.UploadUrl);
            }

            return new Response<PaginatedList<PastExamDto>>()
            {
                Data = result,
                Success = true
            };
        }
    }
}

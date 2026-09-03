using Revisio.Application.Common.Models;
using MediatR;
using Revisio.Application.Performance.Dto;
using Revisio.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using Revisio.Application.Common.Exceptions;
using Revisio.Domain.Enums;
namespace Revisio.Application.Performance.CourseTopicPERFORMANCE
{
    public class CourseTopicQueryHandler : IRequestHandler<CourseTopicQuery, Response<CourseTopicDto>>
    {
        private readonly IAppDbContext context;
        private readonly ICurrentUserService userService;
        public CourseTopicQueryHandler(IAppDbContext context, ICurrentUserService userService)
        {
            this.userService = userService;
            this.context = context;
        }
        public async Task<Response<CourseTopicDto>> Handle(CourseTopicQuery request, CancellationToken cancellationToken)
        {
            var course = await context.Courses.FirstOrDefaultAsync(x => x.UserId == userService.UserId && x.Id == request.CourseId);
            if (course == null)
                throw new NotFoundException("Course not found");
            var answers = await context.ExamSessionAnswers
      .Include(a => a.Questions)
      .Where(a => a.Questions.GenerationRequest.CourseId == request.CourseId
          && a.ExamSession.UserId == userService.UserId)
      .Select(a => new
      {
          Topic = a.Questions.Topic,
          Score = a.Score ?? 0,
          MaxScore = a.Questions.MaxScore
      })
      .ToListAsync(cancellationToken);

            var topicsAccuracy = answers
                .GroupBy(x => x.Topic)
                .Select(g => new Topics
                {
                    Topic = g.Key,
                    Accuracy = g.Sum(x => x.MaxScore) > 0
                        ? Math.Round((g.Sum(x => x.Score) / g.Sum(x => x.MaxScore)) * 100, 0)
                        : 0
                })
                .ToList();
            return new Response<CourseTopicDto>
            {
                Success = true,
                Data = new CourseTopicDto { TopicsAccuracy = topicsAccuracy,CourseId = request.CourseId,CourseName=course.CourseName }
            };

        }
    }
}

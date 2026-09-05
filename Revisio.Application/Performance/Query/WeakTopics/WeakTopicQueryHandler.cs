using MediatR;
using Microsoft.EntityFrameworkCore;
using Revisio.Application.Common.Interfaces;
using Revisio.Application.Common.Models;
using Revisio.Application.Performance.Dto;
namespace Revisio.Application.Performance.Query.WeakTopics
{
    public class WeakTopicQueryHandler : IRequestHandler<WeakTopicQuery, Response<WeakReviewDto>>
    {
        private readonly IAppDbContext context;
        private readonly ICurrentUserService userService;
        public WeakTopicQueryHandler (IAppDbContext context, ICurrentUserService userService)
        {
            this.userService = userService;
            this.context = context;
        }
        public async Task<Response<WeakReviewDto>> Handle(WeakTopicQuery request, CancellationToken cancellationToken)
        {
            var answers = await context.ExamSessionAnswers
                 .Include(x => x.Questions)
                   .ThenInclude(x => x.GenerationRequest )
                 .Where(x => x.Questions.GenerationRequest.CourseId == request.CourseId &&
                 x.Questions.GenerationRequest.UserId == userService.UserId
                 && x.Questions.GenerationRequest.GenrateExamStatus == Domain.Enums.GenrateExamStatus.Completed)
                 .Select(x => new
                 {
                     x.Questions.Topic,
                     x.Questions.MaxScore,
                     x.Score,
                     x.Questions.Lectures.LecName
                 }).ToListAsync(cancellationToken);

            var weakTopics = answers.GroupBy(x => new { x.LecName, x.Topic })
                 .Select(g => new
                 {
                     g.Key.Topic,
                     g.Key.LecName,
                     Accuracy = g.Sum(x => x.MaxScore) > 0
                         ? Math.Round(((decimal)g.Sum(x => x.Score) / g.Sum(x => x.MaxScore)) * 100, 0)
                         : 0
                 }).Where(x => x.Accuracy < 70)
                 .OrderBy(x => x.Accuracy)
                  .Select(x => new WeakTopicDto
                  {
                      Topic = x.Topic,
                      LectureName = x.LecName,
                      Accuracy = x.Accuracy,
                      Status = x.Accuracy < 50 ? "Weak" : "Review"
                  }).ToList();
            return new Response<WeakReviewDto>()
            {
                Success = true,
                Data = new WeakReviewDto() { WeakTopicDto = weakTopics }
            };
        }
    }
}

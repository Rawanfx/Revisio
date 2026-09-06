using Microsoft.EntityFrameworkCore;
using Revisio.Application.Common.Interfaces;
using Revisio.Application.Performance.Dto;
using Revisio.Infrastructure.Data;

namespace Revisio.Infrastructure.Services
{
    public class PerformanceService : ITopicPerformanceService
    {
        private readonly IAppDbContext context;
        public PerformanceService(IAppDbContext context)
        {
            this.context = context;
        }
        public async Task<List<TopicPerformanceDetail>> TopicPerformance(Guid courseId, string userId,CancellationToken cancellationToken)
        {
            var answers = await context.ExamSessionAnswers
              .Include(x => x.Questions)
                  .ThenInclude(x => x.GenerationRequest)
              .Where(x => x.Questions.GenerationRequest.CourseId == courseId
                  && x.Questions.GenerationRequest.UserId == userId
                  && x.Questions.GenerationRequest.GenrateExamStatus == Domain.Enums.GenrateExamStatus.Completed)
              .Select(x => new
              {
                  x.Questions.Topic,
                  x.Questions.MaxScore,
                  Score = x.Score ?? 0,
                  x.Questions.Lectures.LecName
              })
              .ToListAsync(cancellationToken);

            return answers
                .GroupBy(x => new { x.LecName, x.Topic })
                .Select(g => new TopicPerformanceDetail
                {
                    Topic = g.Key.Topic,
                    LectureName = g.Key.LecName,
                    Accuracy = g.Sum(x => x.MaxScore) > 0
                        ? Math.Round((g.Sum(x => x.Score) / g.Sum(x => x.MaxScore)) * 100, 0)
                        : 0,
                    MissedCount = g.Count(x => x.Score < x.MaxScore),
                    TotalAttempted = g.Count()
                })
                .OrderBy(x => x.Accuracy)
                .ToList();
        }
    }
}

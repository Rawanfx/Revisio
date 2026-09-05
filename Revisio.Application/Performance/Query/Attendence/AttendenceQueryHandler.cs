using MediatR;
using Microsoft.EntityFrameworkCore;
using Revisio.Application.Common.Interfaces;
using Revisio.Application.Common.Models;
using Revisio.Application.Performance.Dto;
namespace Revisio.Application.Performance.Query.Attendence
{
    public class AttendenceQueryHandler : IRequestHandler<AttendenceQuery, Response<AttendenceDto>>
    {
        private readonly IAppDbContext context;
        private readonly ICurrentUserService userService;
        public AttendenceQueryHandler (IAppDbContext context,ICurrentUserService userService)
        {
            this.context = context;
            this.userService = userService;
        }
        public async Task<Response<AttendenceDto>> Handle(AttendenceQuery request, CancellationToken cancellationToken)
        {
            var scores = await context.ExamSessionAnswers
                 .Include(x => x.Questions)
                 .ThenInclude(x => x.GenerationRequest)
                 .Where(x => x.Questions.GenerationRequest.CourseId == request.CourseId
                 && x.Questions.GenerationRequest.UserId == userService.UserId)
                 .Select(x => new
                 {
                   studentStudent=  x.Score,
                   questionAnswer = x.Questions.MaxScore
                 }).ToListAsync();
            var questionNum = scores.Count;
            var accuracy = Math.Round((scores.Sum(x => x.studentStudent ?? 0) / scores.Sum(x => x.questionAnswer)) / 100, 2);

            var activityDates = await context.ExamSessionAnswers
                .Include(x => x.ExamSession)
                .Where(x => x.ExamSession.UserId == userService.UserId)
                .Select(x => x.SubmitedAt.Date)
                .Distinct()
                .OrderByDescending(x => x)
                .ToListAsync(cancellationToken);

            var streak = CalculateStreak(activityDates);

            return new Response<AttendenceDto>()
            {
                Success = true,
                Data = new AttendenceDto() { Accuracy = accuracy, QuestionNum = questionNum, Streak = streak }
            };
        }
        private int CalculateStreak (List<DateTime> dates)
        {
            if (!dates.Any())
                return 0;

            var today = DateTime.UtcNow.Date;
            var lastDay = dates.First();

            if (today > lastDay)
                return 0;
            var expectedDay = lastDay.AddDays(-1);
            int streak = 0;
            for (int i = 1; i < dates.Count; i++)
            {
                if (expectedDay == dates[i])
                {
                    streak++;
                    expectedDay = dates[i].AddDays(-1);
                }
                else break;
                   
            }
            return streak;
        }
    }
}

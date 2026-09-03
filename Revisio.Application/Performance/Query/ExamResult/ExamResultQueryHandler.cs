using Revisio.Application.Common.Models;
using MediatR;
using Revisio.Application.Questions.Dto;
using Revisio.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using Revisio.Application.Common.Exceptions;
using System.ComponentModel.DataAnnotations;

namespace Revisio.Application.Performance.Query.ExamResult
{
    public class ExamResultQueryHandler:IRequestHandler<ExamResultQuery,Response<ExamResultDto>>
    {
        private readonly IAppDbContext context;
        private readonly ICurrentUserService userService;
        public ExamResultQueryHandler (IAppDbContext context,ICurrentUserService userService)
        {
            this.userService = userService;
            this.context = context;
        }

        public async Task<Response<ExamResultDto>> Handle(ExamResultQuery request, CancellationToken cancellationToken)
        {
            var examSession = await context.ExamSessions
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == request.ExamSessionId && x.UserId == userService.UserId);
            if (examSession is null)
                throw new NotFoundException("Exam session not found");
            if (examSession.CompletedAt is Nullable<DateTime>)
                throw new ValidationException("This exam ");
            decimal? totalScore = examSession.TotalScore;
            var maxPossibleScoreList = await context.Questions
                .AsNoTracking()
                .Where(x => x.GenerationRequestId == examSession.GenerationRequestId)
                .Select(x => x.MaxScore)
                .ToListAsync();
            decimal maxPossibleScore = maxPossibleScoreList.Sum(x => x);
            int totalQuestions = maxPossibleScoreList.Count();
            var correctAnswerCount = await context.ExamSessionAnswers
                .AsNoTracking()
                .Where(x => x.ExamSessionId == request.ExamSessionId)
                .CountAsync(x => x.IsCorrect == true && x.IsCorrect != null);

            var questionReview = await context.ExamSessionAnswers
                .AsNoTracking()
                .Include(x=>x.Questions)
                  .ThenInclude(x=>x.QuestionOptions)
                  .Include(x=>x.QuestionOptions)
                 .Where(x => x.ExamSessionId == request.ExamSessionId)
                 .Select(z => new QuestionReviewDto
                 {
                     Text=z.Questions.Text,
                     IsCorrect=z.IsCorrect,
                     Type=z.Questions.Type,
                     Score=z.Score,
                     UserAnswerText=z.UserAnswerEsaay,
                     CorrectAnswerText=z.Questions.ModelAnswer,
                     UserAnswerOption = z.QuestionOptions.Option,
                     Explanation = z.Questions.Explanation,
                     CorrectOption= z.Questions.QuestionOptions.FirstOrDefault(y => y.IsCorrect) != null ?
                      z.Questions.QuestionOptions.FirstOrDefault(y => y.IsCorrect).Option : null
                 }).ToListAsync();
            return new Response<ExamResultDto>()
            {
                Success = true,
                Data = new ExamResultDto()
                {
                    CorrectAnswerCount = correctAnswerCount,
                    MaxPossibleScore = maxPossibleScore,
                    QuestionReview = questionReview,
                    TotalQuestion = totalQuestions,
                    TotalScore = totalScore
                }
            };
        }
    }
}

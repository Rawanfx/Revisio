using MediatR;
using Microsoft.EntityFrameworkCore;
using Revisio.Application.Common.Exceptions;
using Revisio.Application.Common.Interfaces;
using Revisio.Application.Common.Models;
using Revisio.Application.Questions.Dto;
using Revisio.Domain.Entities;
using Revisio.Domain.Enums;
using System.ComponentModel.DataAnnotations;
namespace Revisio.Application.Questions.Command.SubmitAnswer
{
    public class SubmitAndNextQuestionCommandHandler : IRequestHandler<SubmitAndNextQuestionCommand, Response<SubmitAndNextQuestionResponse>>
    {
        private readonly IAppDbContext context;
        private readonly ICurrentUserService userService;
        public SubmitAndNextQuestionCommandHandler(IAppDbContext context,ICurrentUserService userService)
        {
            this.context = context;
            this.userService = userService;
        }

        public async Task<Response<SubmitAndNextQuestionResponse>> Handle(SubmitAndNextQuestionCommand request, CancellationToken cancellationToken)
        {
            var generatedRequest = await context.GenerationRequests
                .FirstOrDefaultAsync(x => x.UserId == userService.UserId && x.Id == request.GenerationRequestId);
         
            var startQuiz = await context.ExamSessions
                .FirstOrDefaultAsync(x => x.Id == request.StartQuizId
                && x.UserId == userService.UserId
                && x.GenerationRequestId == request.GenerationRequestId);

            var question = await context.Questions
                .Include (x=>x.QuestionOptions )
                .FirstOrDefaultAsync(x => x.Id == request.QuestionId && x.GenerationRequestId== request.GenerationRequestId);
            if (question ==null|| startQuiz==null ||generatedRequest == null)
                throw new NotFoundException("Invalid data");

            decimal? score = 0;
            bool isCorrect = false;
            switch (question.Type)
            {
                case QuestionType.MCQ:
                case QuestionType.TrueFalse:
                    if (request.McqAnswer is null)
                        throw new ValidationException("you muust select a option");
                    var correctOption = question.QuestionOptions.FirstOrDefault(o => o.IsCorrect);
                    var selectedAnswer = question.QuestionOptions.Any(x => x.Id == request.McqAnswer);
                    if (!selectedAnswer)
                        throw new ValidationException("this option not found");
                     isCorrect = correctOption?.Id == request.McqAnswer;
                    score = isCorrect ? question.MaxScore : 0;
                    break;
                case QuestionType.Essay:
                    if (request.EssayAnswer is null)
                        throw new ValidationException("this question must have an essay answer");
                    //calculate score from ai
                    break;
            }
            ExamSessionAnswer examSessionAnswer = new ExamSessionAnswer()
            {
                ExamSessionId = startQuiz.Id,
                Id = Guid.NewGuid(),
                QuestionId = question.Id,
                Score = score,
                TimeTakeForAnswer = request.TimeTakeToAnswer,
                UserAnswerEsaay = request.EssayAnswer,
                UserAnswerOption = request.McqAnswer
            };
             context.ExamSessionAnswers.Add(examSessionAnswer);
           await context.SaveChangesAsync(cancellationToken);
            int newIndex = 0;
            if (generatedRequest.TotalQuestions > question.Index)
                newIndex = question.Index + 1;
            SubmitAndNextQuestionResponse response;
            if (newIndex == 0)
            {
                startQuiz.CompletedAt = DateTime.UtcNow;
                var allAnswer = await context.ExamSessionAnswers
                    .Where(x => x.ExamSessionId == startQuiz.Id)
                    .Select(x=>x.Score)
                    .ToListAsync(cancellationToken);
                startQuiz.TotalScore= allAnswer.Sum(x => x??0 );
                await context.SaveChangesAsync(cancellationToken);
                response = new SubmitAndNextQuestionResponse()
                {
                    IsCompleted =true,
                    IsCorrect = isCorrect,
                    Explanation = question.Explanation,
                    questionData= null
                };
            }
            else
            {
                var nextQuestionData = await context.Questions
          .Where(x => x.Index == newIndex && x.GenerationRequestId == request.GenerationRequestId)
          .Select(y => new questionData
          {
              Index = newIndex,
              OptionsId = y.QuestionOptions.Select(z => z.Id).ToList(),
              QuestionType = y.Type,
              Text = y.Text
          })
          .FirstOrDefaultAsync(cancellationToken);
                response = new SubmitAndNextQuestionResponse
                {
                    IsCompleted = false,
                    IsCorrect = isCorrect,
                    Explanation = question.Explanation,
                    questionData = nextQuestionData
                };
            }
            return new Response<SubmitAndNextQuestionResponse>
            {
                Data = response,
                Success = true
            };
        }
    }
}

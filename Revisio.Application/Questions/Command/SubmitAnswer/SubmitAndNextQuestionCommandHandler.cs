using Revisio.Application.Common.Exceptions;
using Revisio.Application.Questions.Command.SubmitAnswer;
using Revisio.Application.Questions.Dto;
using Revisio.Domain.Entities;
using Revisio.Domain.Enums;
using Revisio.Application.Common.Models;
using System.ComponentModel.DataAnnotations;
using Revisio.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using MediatR;
public class SubmitAndNextQuestionCommandHandler:IRequestHandler<SubmitAndNextQuestionCommand,Response<SubmitAndNextQuestionResponse>>
{
    private readonly IAppDbContext context;
    private readonly ICurrentUserService userService;
    public SubmitAndNextQuestionCommandHandler(IAppDbContext context,ICurrentUserService currentUser)
    {
        this.context = context;
        this.userService = currentUser;
    }

    public async Task<Response<SubmitAndNextQuestionResponse>> Handle(SubmitAndNextQuestionCommand request, CancellationToken cancellationToken)
    {

        var generatedRequest = await context.GenerationRequests
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.UserId == userService.UserId && x.Id == request.GenerationRequestId, cancellationToken);

        var startQuiz = await context.ExamSessions
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == request.StartQuizId
            && x.UserId == userService.UserId
            && x.GenerationRequestId == request.GenerationRequestId, cancellationToken);
        var question = await context.Questions
            .AsNoTracking()
            .Include(x => x.QuestionOptions)
            .FirstOrDefaultAsync(x => x.Id == request.QuestionId && x.GenerationRequestId == request.GenerationRequestId, cancellationToken);
        if (question is null || startQuiz is null || generatedRequest is null)
            throw new NotFoundException("Invalid data");

        decimal? score = 0;
        bool isCorrect = false;
        switch (question.Type)
        {
            case QuestionType.MCQ:
            case QuestionType.TrueFalse:
                if (request.McqAnswer is null)
                    throw new ValidationException("you must select an option");
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
                break;
        }

        var examSessionAnswer = new ExamSessionAnswer
        {
            ExamSessionId = startQuiz.Id,
            Id = Guid.NewGuid(),
            QuestionId =question.Id,
            Score = 5,
            TimeTakeForAnswer = request.TimeTakeToAnswer,
            UserAnswerEsaay = request.EssayAnswer,
            UserAnswerOption = request.McqAnswer
        };
        context.ExamSessionAnswers.Add(examSessionAnswer);
      var rowAffcted=  await context.SaveChangesAsync(cancellationToken);
        Console.WriteLine($"Row affected {rowAffcted}");
        int newIndex = 0;
        if (generatedRequest.TotalQuestions > question.Index)
            newIndex = question.Index + 1;

        SubmitAndNextQuestionResponse response;
        if (newIndex == 0)
        {
            startQuiz.CompletedAt = DateTime.UtcNow;
            var allAnswer = await context.ExamSessionAnswers
                .Where(x => x.ExamSessionId == startQuiz.Id)
                .Select(x => x.Score)
                .ToListAsync(cancellationToken);
            startQuiz.TotalScore = allAnswer.Sum(x => x ?? 0);
            await context.SaveChangesAsync(cancellationToken);

            response = new SubmitAndNextQuestionResponse
            {
                IsCompleted = true,
                IsCorrect = isCorrect,
                Explanation = question.Explanation,
                questionData = null
            };
        }
        else
        {
            var nextQuestionRaw = await context.Questions
                .Include(x => x.QuestionOptions)
                .Where(x => x.Index == newIndex && x.GenerationRequestId == request.GenerationRequestId)
                .Select(y => new
                {
                    y.Type,
                    y.Text,
                    y.Id,
                    options = y.QuestionOptions.Select(z => new { z.Id, z.Option }).ToList()
                }).FirstOrDefaultAsync(cancellationToken);

            if (nextQuestionRaw is null)
                throw new NotFoundException("Question not found");

            var nextQuestionData = new questionData
            {
                Text = nextQuestionRaw.Text,
                Index = newIndex,
                Options = nextQuestionRaw.options.ToDictionary(x => x.Id, x => x.Option),
                QuestionId = nextQuestionRaw.Id,
                QuestionType = nextQuestionRaw.Type
            };

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
           // Data = response,
            Success = true
        };
    }
}
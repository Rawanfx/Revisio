using MassTransit;
using Microsoft.EntityFrameworkCore;
using Revisio.Application.Common.Interfaces;
using Revisio.Application.Common.Models;
using Revisio.Application.Events;
using Revisio.Domain.Entities;
using Revisio.Domain.Enums;
using Serilog;

namespace Revisio.Infrastructure.Consumers
{
    public class QuestionGenerationConsumer : IConsumer<QuestionGeneratedRequest>
    {
        private readonly IExamAIGenerator aiServiceClient;
        private readonly IAppDbContext dbContext;

        public QuestionGenerationConsumer(IExamAIGenerator aiServiceClient, IAppDbContext dbContext)
        {
            this.aiServiceClient = aiServiceClient;
            this.dbContext = dbContext;
        }

        public async Task Consume(ConsumeContext<QuestionGeneratedRequest> context)
        {
            var generatedQuestionId = context.Message.GenerateRequestId;
            var ct = context.CancellationToken;

            var request = await dbContext.GenerationRequests
                .Include(r => r.SelectedLectures)
                    .ThenInclude(sl => sl.Lectures)
                .FirstOrDefaultAsync(r => r.Id == generatedQuestionId, ct);

            if (request is null)
            {
                Log.Error($"generatedQuestionId not found {generatedQuestionId}");
                return; 
            }

            try
            {
                var lectures = request.SelectedLectures.Select(sl => sl.Lectures).ToList();
                var lectureDistribution = DistributeQuestions(lectures, request.TotalQuestions);

                var aiRequestDto = new GenerateQuestionsAIServiceRequestDto
                {
                    Lectures = lectureDistribution,
                    TotalQuestion = request.TotalQuestions,
                    DifficultyBreakdown = new DifficultyBreakdownDto
                    {
                        Easy = request.EasyQuestionNum,
                        Medium = request.MediumQuestionNum,
                        Hard = request.HardQuestionNum
                    },
                    TypeBreakdown = new TypeBreakdownDto
                    {
                        MCQ = request.MCQQuestionNum,
                        Essay = request.EssayQuestionNum,
                        TrueFalse = request.TrueFalseQuestionNum
                    }
                };

                var generatedQuestions = await aiServiceClient.GenerateQuestions(aiRequestDto, ct);

                int index = 1;

                foreach (var gq in generatedQuestions.Questions)
                {
                    var question = new Questions
                    {
                        Id = Guid.NewGuid(),
                        GenerationRequestId = request.Id,
                        Text = gq.Text,
                        Explanation = gq.Explanation,
                        Topic = gq.Topic,
                        Type = Enum.Parse<QuestionType>(gq.Type, ignoreCase: true),
                        Difficulty = Enum.Parse<QuestionDifficulty>(gq.Difficulty, ignoreCase: true),
                        QuestionOptions = gq.Options.Select(o => new QuestionOptions
                        {
                            Id = Guid.NewGuid(),
                            Option = o.Text,
                            IsCorrect = o.IsCorrect
                        }).ToList(),
                        Index=index
                    };
                    dbContext.Questions.Add(question);
                    index++;
                }

                request.GenrateExamStatus = GenrateExamStatus.Completed;
                await dbContext.SaveChangesAsync(ct);
            }
            catch (Exception)
            {
                request.GenrateExamStatus = GenrateExamStatus.Failed;
                await dbContext.SaveChangesAsync(ct);
                throw;
            }
        }

        private List<LectureContentDto> DistributeQuestions(List<Lectures> lectures, int totalQuestions)
        {
            var baseCount = totalQuestions / lectures.Count;
            var remainder = totalQuestions % lectures.Count;
            return lectures.Select((lecture, index) => new LectureContentDto
            {
                LectureId = lecture.Id,
                QuestionsCount = baseCount + (index < remainder ? 1 : 0)
            }).ToList();
        }
    }
}
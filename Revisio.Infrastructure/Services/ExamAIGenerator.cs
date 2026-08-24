using Revisio.Application.Common.Interfaces;
using Revisio.Application.Common.Models;
using Revisio.Application.Events;
using Revisio.Application.Questions.Dto;
using Revisio.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
namespace Revisio.Infrastructure.Services
{
    public class ExamAIGenerator : IExamAIGenerator
    {
        private readonly ExamAIService.ExamAIServiceClient client;
        public ExamAIGenerator(ExamAIService.ExamAIServiceClient client){
            this.client = client;
            }
        public async Task<GenerateQuestionsAIServiceResponseDto> GenerateQuestions(GenerateQuestionsAIServiceRequestDto dto, CancellationToken cancellationToken)
        {
            var request = new GenerateQuestionsRequest()
            {
                DifficultyBreakdown = new DifficultyBreakdown()
                {
                    Easy = dto.DifficultyBreakdown.Easy,
                    Hard = dto.DifficultyBreakdown.Hard,
                    Medium = dto.DifficultyBreakdown.Medium
                },
                TotalQuestions = dto.TotalQuestion,
                TypeBreakdown = new TypeBreakdown()
                {
                    Essay= dto.TypeBreakdown.Essay,
                    Mcq=dto.TypeBreakdown.MCQ,
                    TrueFalse=dto.TypeBreakdown.TrueFalse
                }
            };
            request.Lectures.AddRange(dto.Lectures.Select(l => new LectureContent
            {
                LectureId = l.LectureId.ToString(),   // Guid → string
                QuestionsCount = l.QuestionsCount
            }));
            var response =await  client.GenerateExamAsync(request, cancellationToken: cancellationToken);
            if (!response.Success)
                throw new Exception($"AI generation failed: {response.ErrorMessage}");
            var questions= response.Questions.Select(q => new GeneratedQuestionsResponseDto
            {
                Text = q.Text,
                Type = q.Type,
                Difficulty = q.Difficulty,
                Topic = q.Topic,
                Explanation = q.Explanation,
                ModelAnswer = string.IsNullOrEmpty(q.ModelAnswer) ? null : q.ModelAnswer,
                GradingCriteria = q.GradingCriteria.Count > 0 ? q.GradingCriteria.ToList() : null,
                Options = q.Options.Select(o => new GeneratedOptionDto
                {
                    Text = o.Text,
                    IsCorrect = o.IsCorrect
                }).ToList()
            }).ToList();
            return new GenerateQuestionsAIServiceResponseDto() { Success = true, Questions = questions };
        }

        public async Task<GradeAnswerResultDto> GradeAnswerAsync(string lecture_id, string student_answer, string question_text, int max_score, string model_answer, string grading_criteria,CancellationToken cancellationToken)
        {
            var request = new GradeAnswerRequest()
            {
               GradingCriteria=grading_criteria,
               LectureId=lecture_id,
               MaxScore=max_score,
               ModelAnswer=model_answer,
               QuestionText=question_text,
               StudentAnswer=student_answer,
               
            };
            var response = await client.GradeAnswerAsync(request, cancellationToken: cancellationToken);
            GradeAnswerResultDto result = new GradeAnswerResultDto()
            {
                confidence = response.Confidence,
                Error_message = response.ErrorMessage,
                feedback = response.Feedback,
                Missing_point = response.MissingPoint.ToList(),
                Score = (decimal)response.Score,
                Success = response.Success
            };
            return result;
        }

        public async Task<bool> IndexLectureAsync(string content, string userId, Guid lectureId, Guid courseId,CancellationToken cancellationToken)
        {
            var request = new IndexLectureRequest()
            {
                Content = content,
                CourseId = courseId.ToString(),
                LectureId = lectureId.ToString(),
                UserId=userId
            };
          var response =  await client.IndexLectureAsync(request,cancellationToken: cancellationToken);
            return response.Success;
        }
    }
}

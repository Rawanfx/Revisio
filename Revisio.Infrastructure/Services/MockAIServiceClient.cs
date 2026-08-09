using Revisio.Application.Common.Interfaces;
using Revisio.Application.Common.Models;

namespace Revisio.Infrastructure.Services
{
    public class MockAIServiceClient : IGenerateQuestionAIService
    {
        public List<GeneratedQuestionsDto> GenerateQuestions(GenerateQuestionsAIServiceRequestDto dto)
        {
            var questions = new List<GeneratedQuestionsDto>();

            for (int i = 0; i < dto.TotalQuestion; i++)
            {
                questions.Add(new GeneratedQuestionsDto
                {
                    Text = $"Mock Question Number {i+1}",
                    Type = "MCQ",
                    Difficulty = "Medium",
                    Topic = "General",
                    Explanation = "ده شرح وهمي للاختبار",
                    Options = new List<GeneratedOptionDto>
                    {
                        new() { Text = "a.", IsCorrect = true },
                        new() { Text = "b.", IsCorrect = false },
                        new() { Text = "c.", IsCorrect = false },
                        new() { Text = "d.", IsCorrect = false }
                    }
                });
            }
            return questions;
        }
    }
}

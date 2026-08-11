using Revisio.Application.Common.Interfaces;
using Revisio.Application.Common.Models;

namespace Revisio.Infrastructure.Services
{
    public class MockAIServiceClient : IExamAIGenerator
    {
        public async Task<List<GeneratedQuestionsResponseDto>> GenerateQuestions(GenerateQuestionsAIServiceRequestDto request,CancellationToken ct)
        {
            var questions = new List<GeneratedQuestionsResponseDto>();

            for (int i = 0; i < request.TotalQuestion; i++)
            {
                questions.Add(new GeneratedQuestionsResponseDto
                {
                    Text = $"Mock Question Number {i+1}",
                    Type = "MCQ",
                    Difficulty = "Meduim",
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

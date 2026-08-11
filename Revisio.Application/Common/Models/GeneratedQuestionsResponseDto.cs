
namespace Revisio.Application.Common.Models
{
    public class GenerateQuestionsAIServiceResponseDto
    {
        public bool Success { get; set; }
        public string? ErrorMessage { get; set; }
        public List<GeneratedQuestionsResponseDto> Questions { get; set; }
    }
    public class GeneratedQuestionsResponseDto
    {
        public string Text { get; set; }
        public string Type { get; set; }         // "MCQ", "TrueFalse", "Essay"
        public string Difficulty { get; set; }    // "Easy", "Medium", "Hard"
        public string Topic { get; set; }
        public string Explanation { get; set; }
        public List<GeneratedOptionDto> Options { get; set; }
        public string? ModelAnswer { get; set; }                 
        public List<string>? GradingCriteria { get; set; }
    }
    public class GeneratedOptionDto
    {
        public string Text { get; set; }
        public bool IsCorrect { get; set; }
    }
}

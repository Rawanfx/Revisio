using Revisio.Domain.Enums;

namespace Revisio.Application.Questions.Dto
{
    public class ExamResultDto
    {
        public decimal? TotalScore { get; set; }
        public decimal MaxPossibleScore { get; set; }
        public int TotalQuestion { get; set; }
        public int CorrectAnswerCount { get; set; }
        public List<QuestionReviewDto> QuestionReview { get; set; }
    }
    public class QuestionReviewDto
    {
        public string Text { get; set; }
        public QuestionType Type { get; set; }
        public string? UserAnswerText { get; set; }  
        public string? UserAnswerOption { get; set; }  
        public string? CorrectAnswerText { get; set; }
        public bool? IsCorrect { get; set; }
        public decimal? Score { get; set; }
        public string Explanation { get; set; }
        public string? CorrectOption { get; set; }
    }
}

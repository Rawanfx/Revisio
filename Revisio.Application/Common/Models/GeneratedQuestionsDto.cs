
namespace Revisio.Application.Common.Models
{
    public class GeneratedQuestionsDto
    {
        public string Text { get; set; }
        public string Type { get; set; }         // "MCQ", "TrueFalse", "Essay"
        public string Difficulty { get; set; }    // "Easy", "Medium", "Hard"
        public string Topic { get; set; }
        public string Explanation { get; set; }
        public List<GeneratedOptionDto> Options { get; set; }
    }
    public class GeneratedOptionDto
    {
        public string Text { get; set; }
        public bool IsCorrect { get; set; }
    }
}

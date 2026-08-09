namespace Revisio.Application.Common.Models
{
    public class GenerateQuestionsAIServiceRequestDto
    {
        public List<LectureContentDto> Lectures { get; set; }
        public int TotalQuestion { get; set; }
        public DifficultyBreakdownDto DifficultyBreakdown { get; set; }
        public TypeBreakdownDto TypeBreakdown { get; set; }
    }
    public class LectureContentDto
    {
        public Guid LectureId { get; set; }
        public string Content { get; set; }       
        public int QuestionsCount { get; set; }
    }
    public class DifficultyBreakdownDto
    {
        public int Easy { get; set; }
        public int Meduim { get; set; }
        public int Hard { get; set; }
    }
    public class TypeBreakdownDto
    {
        public int MCQ { get; set; }
        public int Essay { get; set; }
        public int TrueFalse { get; set; }
    }
}

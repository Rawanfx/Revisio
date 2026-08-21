using Revisio.Domain.Enums;

namespace Revisio.Application.Questions.Dto
{
    public class StartQuizResponse
    {
        public Guid ExamSessionId { get; set; }
        public questionData questionData { get; set; } = new();
    }
    public class questionData
    {
        public string Text { get; set; }
        public Dictionary<Guid,string>? Options { get; set; }
        public QuestionType QuestionType { get; set; }
        public int Index { get; set; }
        public Guid QuestionId { get; set; }

    }
}

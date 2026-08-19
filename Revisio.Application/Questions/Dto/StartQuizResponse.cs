using Revisio.Domain.Enums;

namespace Revisio.Application.Questions.Dto
{
    public class StartQuizResponse
    {
        public Guid ExamSessionId { get; set; }
        public string Text { get; set; }
        public List<string>?Options { get; set; }
        public QuestionType QuestionType { get; set; }
        public int Index { get; set; }
    }
}

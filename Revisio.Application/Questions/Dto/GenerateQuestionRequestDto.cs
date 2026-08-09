
using Revisio.Domain.Enums;

namespace Revisio.Application.Questions.Dto
{
    public class GenerateQuestionRequestDto
    {
        public List<Guid> LectureIds { get; set; }
        public int TotalQuestions { get; set; }
        public ExamMode ExamMode { get; set; }
        public int Easy { get; set; }
        public int Meduim { get; set; }
        public int Hard { get; set; }
        public int MCQ { get; set; }
        public int Essay { get; set; }
        public int TrueFalse { get; set; }
    }
}

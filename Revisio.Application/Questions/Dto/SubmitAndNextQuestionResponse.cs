
namespace Revisio.Application.Questions.Dto
{
    public class SubmitAndNextQuestionResponse
    {
        public bool? IsCorrect { get; set; }
        public questionData? questionData { get; set; }
        public bool IsCompleted { get; set; }
        public string? Explanation { get; set; }

    }
}

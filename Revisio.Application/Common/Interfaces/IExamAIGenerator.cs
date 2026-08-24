using Revisio.Application.Common.Models;
using Revisio.Application.Questions.Dto;

namespace Revisio.Application.Common.Interfaces
{
    public interface IExamAIGenerator
    {
       Task<GenerateQuestionsAIServiceResponseDto> GenerateQuestions(GenerateQuestionsAIServiceRequestDto dto, CancellationToken cancellationToken);
        Task<bool> IndexLectureAsync(string content, string userId, Guid lectureId, Guid courseId,CancellationToken cancellationToken);
        Task<GradeAnswerResultDto> GradeAnswerAsync(string lecture_id, string student_answer, string question_text, int max_score, string model_answer, string grading_criteria,CancellationToken cancellationToken);
    }
}

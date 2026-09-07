using Revisio.Application.Common.Models;
using Revisio.Application.Questions.Dto;
using Revisio.Application.Performance.Dto;
using Revisio.Application.ExplainMore.Dto;
namespace Revisio.Application.Common.Interfaces
{
    public interface IExamAIGenerator
    {
       Task<GenerateQuestionsAIServiceResponseDto> GenerateQuestions(GenerateQuestionsAIServiceRequestDto dto, CancellationToken cancellationToken);
        Task<bool> IndexLectureAsync(string content, string userId, Guid lectureId, Guid courseId,CancellationToken cancellationToken);
        Task<GradeAnswerResultDto> GradeAnswerAsync(string lecture_id, string student_answer, string question_text, int max_score, string model_answer, string grading_criteria,CancellationToken cancellationToken);
        Task<GeneatePreExamSummaryDto> GeneratePreExamSummary(GeneratePreExamSummaryRequest request,CancellationToken cancellationToken);
        Task<ExplainTextResponse> GenerateExplainText(string lecture_id, string selected_text, string question_text,CancellationToken cancellationToken);
    }
}

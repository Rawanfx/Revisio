using Revisio.Application.Common.Models;

namespace Revisio.Application.Common.Interfaces
{
    public interface IExamAIGenerator
    {
       Task<GenerateQuestionsAIServiceResponseDto> GenerateQuestions(GenerateQuestionsAIServiceRequestDto dto, CancellationToken cancellationToken);
        Task<bool> IndexLectureAsync(string content, string userId, Guid lectureId, Guid courseId,CancellationToken cancellationToken);
    }
}

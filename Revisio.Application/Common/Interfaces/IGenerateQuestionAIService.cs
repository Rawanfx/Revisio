using Revisio.Application.Common.Models;

namespace Revisio.Application.Common.Interfaces
{
    public interface IGenerateQuestionAIService
    {
       Task< List<GeneratedQuestionsDto>> GenerateQuestions(GenerateQuestionsAIServiceRequestDto dto, CancellationToken cancellationToken);
    }
}

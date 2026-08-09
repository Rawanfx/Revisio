using Revisio.Application.Common.Models;

namespace Revisio.Application.Common.Interfaces
{
    public interface IGenerateQuestionAIService
    {
        List<GeneratedQuestionsDto> GenerateQuestions(GenerateQuestionsAIServiceRequestDto dto);
    }
}

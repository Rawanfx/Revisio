using FluentValidation;
using Revisio.Application.Questions.Dto;

namespace Revisio.Application.Questions.Command.GenerateQuestion
{
    public class GenerateQuestionValidator:AbstractValidator<GenerateQuestionCommand>
    {
        public GenerateQuestionValidator()
        {
            RuleFor(x => x.GenerateQuestionRequestDto.TotalQuestions)
                .GreaterThan(0);

            RuleFor(x => x.GenerateQuestionRequestDto)
                .Must(IsValidTotalQ);

            RuleFor(x => x.GenerateQuestionRequestDto.TrueFalse)
                .GreaterThanOrEqualTo(0);
            RuleFor(x => x.GenerateQuestionRequestDto.Hard)
                .GreaterThanOrEqualTo(0);
            RuleFor(x => x.GenerateQuestionRequestDto.Essay)
                .GreaterThanOrEqualTo(0);
            RuleFor(x => x.GenerateQuestionRequestDto.Easy)
                .GreaterThanOrEqualTo(0);
            RuleFor(x => x.GenerateQuestionRequestDto.MCQ)
                .GreaterThanOrEqualTo(0);
            RuleFor(x => x.GenerateQuestionRequestDto.Medium)
                .GreaterThanOrEqualTo(0);
            RuleFor(x => x.GenerateQuestionRequestDto.LectureIds)
                .NotEmpty();

        }
        private bool IsValidTotalQ(GenerateQuestionRequestDto dto)
        {
            var total = dto.TrueFalse + dto.MCQ + dto.Essay;
            var totalMode = dto.Easy + dto.Medium + dto.Hard;
            return (total == dto.TotalQuestions && totalMode == dto.TotalQuestions);
        }
    }
}

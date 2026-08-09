using FluentValidation;
using Revisio.Application.Questions.Dto;

namespace Revisio.Application.Questions.Command.GenerateQuestion
{
    public class GenerateQuestionValidator:AbstractValidator<GenerateQuestionRequestDto>
    {
        public GenerateQuestionValidator()
        {
            RuleFor(x => x.TotalQuestions)
                .GreaterThan(0);

            RuleFor(x => x)
                .Must(IsValidTotalQ);

            RuleFor(x => x.TrueFalse)
                .GreaterThanOrEqualTo(0);
            RuleFor(x => x.Hard)
                .GreaterThanOrEqualTo(0);
            RuleFor(x => x.Essay)
                .GreaterThanOrEqualTo(0);
            RuleFor(x => x.Easy)
                .GreaterThanOrEqualTo(0);
            RuleFor(x => x.MCQ)
                .GreaterThanOrEqualTo(0);
            RuleFor(x => x.Meduim)
                .GreaterThanOrEqualTo(0);
            RuleFor(x => x.LectureIds)
                .NotEmpty();

        }
        private bool IsValidTotalQ(GenerateQuestionRequestDto dto)
        {
            var total = dto.TrueFalse + dto.MCQ + dto.Essay;
            var totalMode = dto.Easy + dto.Meduim + dto.Hard;
            return (total == dto.TotalQuestions && totalMode == dto.TotalQuestions);
        }
    }
}

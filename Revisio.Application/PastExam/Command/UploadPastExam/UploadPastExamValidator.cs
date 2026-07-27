using FluentValidation;
using Revisio.Application.Common.Validator;

namespace Revisio.Application.PastExam.Command.UploadPastExam
{
    public class UploadPastExamValidator:AbstractValidator<UploadPastExamCommand>
    {
        public UploadPastExamValidator()
        {
            RuleFor(x => x.pastFile.FileName)
                     .NotEmpty().MaximumLength(200);

            RuleFor(x => x.pastFile)
                .NotNull().WithMessage("File is required")
                .Must(FileValidatorHelper.BeAValidFileSize).WithMessage($"File size must not exceed {FileValidatorHelper.MaxFileSizeInBytes / 1024 / 1024} MB")
                .Must(FileValidatorHelper.HaveValidExtension).WithMessage($"Only {string.Join(", ", FileValidatorHelper._allowedExtensions)} files are allowed")
                .MustAsync(FileValidatorHelper.HaveValidFileSignture);
           
        }
    }
}

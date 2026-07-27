using FluentValidation;
using Revisio.Application.Common.Validator;

namespace Revisio.Application.Lecture.Command.UploadLecture
{
    public class UploadLectureValidator:AbstractValidator<UploadLectureCommand>
    {
        public UploadLectureValidator()
        {
            RuleFor(x => x.LectureFile.FileName)
                       .NotEmpty().MaximumLength(200);

            RuleFor(x => x.LectureFile)
                .NotNull().WithMessage("File is required")
                .Must(FileValidatorHelper. BeAValidFileSize).WithMessage($"File size must not exceed {FileValidatorHelper.MaxFileSizeInBytes / 1024 / 1024} MB")
                .Must(FileValidatorHelper. HaveValidExtension).WithMessage($"Only {string.Join(", ",FileValidatorHelper. _allowedExtensions)} files are allowed")
                .MustAsync(FileValidatorHelper.HaveValidFileSignture);
            
        }

    }
}

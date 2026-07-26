using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace Revisio.Application.Lecture.Command.UploadLecture
{
    public class UploadLectureValidator:AbstractValidator<UploadLectureCommand>
    {
        private readonly string[] _allowedExtensions = { ".pdf", ".pptx", ".ppt", ".docx" };
        private const long MaxFileSizeInBytes = 20 * 1024 * 1024; // 20 MB
        private  readonly Dictionary<string, List<byte[]>> FileSignatures = new()
        {
            { ".pdf", new List<byte[]> { new byte[] { 0x25, 0x50, 0x44, 0x46 } } },
            { ".docx", new List<byte[]> { new byte[] { 0x50, 0x4B, 0x03, 0x04 } } },
            { ".pptx", new List<byte[]> { new byte[] { 0x50, 0x4B, 0x03, 0x04 } } },
            { ".ppt", new List<byte[]> { new byte[] { 0xD0, 0xCF, 0x11, 0xE0 } } } 
        };
        public UploadLectureValidator()
        {
            RuleFor(x => x.LectureFile.FileName)
                       .NotEmpty().MaximumLength(200);

            RuleFor(x => x.LectureFile)
                .NotNull().WithMessage("File is required")
                .Must(BeAValidFileSize).WithMessage($"File size must not exceed {MaxFileSizeInBytes / 1024 / 1024} MB")
                .Must(HaveValidExtension).WithMessage($"Only {string.Join(", ", _allowedExtensions)} files are allowed")
                .MustAsync(HaveValidFileSignture);
            
        }
        private async Task<bool> HaveValidFileSignture(IFormFile file,CancellationToken cancellationToken)
        {
            if (file == null)
                return false;
            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!FileSignatures.ContainsKey(extension))
                return false;
            using var stream = file.OpenReadStream();
            var headerBytes = new byte[8];
           await stream.ReadAsync(headerBytes, 0, headerBytes.Length);
            stream.Position = 0;
            return FileSignatures[extension].Any(x => headerBytes.Take(x.Length).SequenceEqual(x));

        }
        private bool BeAValidFileSize(IFormFile file)
        {
            return file != null && file.Length > 0 && file.Length <= MaxFileSizeInBytes;
        }

        private bool HaveValidExtension(IFormFile file)
        {
            if (file == null) return false;
            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            return _allowedExtensions.Contains(extension);
        }
    }
}

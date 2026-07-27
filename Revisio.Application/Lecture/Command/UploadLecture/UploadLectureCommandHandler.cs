using MediatR;
using Microsoft.EntityFrameworkCore;
using Revisio.Application.Common.Exceptions;
using Revisio.Application.Common.Interfaces;
using Revisio.Application.Common.Models;
using Revisio.Domain.Entities;
using System.Security.Cryptography;
namespace Revisio.Application.Lecture.Command.UploadLecture
{
    public class UploadLectureCommandHandler : IRequestHandler<UploadLectureCommand, Response<Guid>>
    {
        private readonly IAppDbContext context;
        private readonly ICurrentUserService currentUser;
        private readonly IUploadToCloud uploadToCloud;
        public UploadLectureCommandHandler(IAppDbContext context
            ,ICurrentUserService currentUser
            ,IUploadToCloud uploadToCloud)
        {
            this.context = context;
            this.currentUser = currentUser;
            this.uploadToCloud = uploadToCloud;
        }
        public async Task<Response<Guid>> Handle(UploadLectureCommand request, CancellationToken cancellationToken)
        {
            var user = currentUser.UserId;
            if (user == null)
                throw new UnauthorizedException("Unauthorized user");
            var course = await context.Courses.FirstOrDefaultAsync(x => x.Id == request.CourseId
            && x.UserId == user);
            if (course == null)
                throw new NotFoundException("Course Not Found");
            string fileHash;
            using (var sha256 = SHA256.Create())
            using (var s = request.LectureFile.OpenReadStream())
            {

                fileHash = BitConverter.ToString(sha256.ComputeHash(s))
                    .Replace("-", "").ToLower();
            }
            if (await context.Lectures.AnyAsync(x => x.FileHash == fileHash && x.CourseId == request.CourseId))
                throw new RepeatException("File has been uploaded!"); 
            using var stream = request.LectureFile.OpenReadStream();
            var fileKey = await uploadToCloud.UploadAsync(stream, request.LectureFile.FileName, request.LectureFile.ContentType);
            var lecture = new Lectures()
            {
                CourseId = request.CourseId,
                LecName = request.LectureFile.FileName,
                UploadedAt = DateTime.UtcNow,
                UploadUrl = fileKey,
                FileHash=fileHash
            };
           await context.Lectures.AddAsync(lecture);
            await context.SaveChangesAsync();
            return new Response<Guid>() {Data= lecture.Id,Success=true, Message="Uploaded Successfully" };
        }
    }
}

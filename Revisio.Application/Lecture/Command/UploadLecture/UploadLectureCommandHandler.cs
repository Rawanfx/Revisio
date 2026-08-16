using MassTransit;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Revisio.Application.Common.Exceptions;
using Revisio.Application.Common.Interfaces;
using Revisio.Application.Common.Models;
using Revisio.Application.Events;
using Revisio.Domain.Entities;
using System.Reflection.Metadata;
using System.Security.Cryptography;
using System.Text.Json;
namespace Revisio.Application.Lecture.Command.UploadLecture
{
    public class UploadLectureCommandHandler : IRequestHandler<UploadLectureCommand,Revisio.Application.Common.Models.Response<Guid>>
    {
        private readonly IAppDbContext context;
        private readonly ICurrentUserService currentUser;
        private readonly IUploadToCloud uploadToCloud;
        private readonly ITextExtractorFactory textExtractorFactory;
        private readonly IPublishEndpoint publishEndpoint;
        public UploadLectureCommandHandler(IAppDbContext context
            ,ICurrentUserService currentUser
            ,IUploadToCloud uploadToCloud
            , ITextExtractorFactory textExtractorFactory
            ,IPublishEndpoint publishEndpoint)
        {
            this.context = context;
            this.currentUser = currentUser;
            this.uploadToCloud = uploadToCloud;
            this.textExtractorFactory = textExtractorFactory;
            this.publishEndpoint = publishEndpoint;
        }
        public async Task<Revisio.Application.Common.Models.Response<Guid>> Handle(UploadLectureCommand request, CancellationToken cancellationToken)
        {
            var user = currentUser.UserId;
            if (user == null)
                throw new UnauthorizedException("Unauthorized user");

            var course = await context.Courses.FirstOrDefaultAsync(x => x.Id == request.CourseId
                && x.UserId == user, cancellationToken);
            if (course == null)
                throw new NotFoundException("Course Not Found");

            byte[] fileBytes;
            using (var tempStream = new MemoryStream())
            {
                await request.LectureFile.OpenReadStream().CopyToAsync(tempStream, cancellationToken);
                fileBytes = tempStream.ToArray();
            }

            string fileHash;
            using (var sha256 = SHA256.Create())
            using (var hashStream = new MemoryStream(fileBytes))
            {
                var hashBytes = await sha256.ComputeHashAsync(hashStream, cancellationToken);
                fileHash = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
            }

            if (await context.Lectures.AnyAsync(x => x.FileHash == fileHash && x.CourseId == request.CourseId, cancellationToken))
                throw new RepeatException("File has been uploaded!");

            string fileKey;
            using (var uploadStream = new MemoryStream(fileBytes))
            {
                fileKey = await uploadToCloud.UploadAsync(uploadStream, request.LectureFile.FileName, request.LectureFile.ContentType);
            }

            var extension = Path.GetExtension(request.LectureFile.FileName);
            var extractor = textExtractorFactory.textExtractor(extension);
            string content;
            using (var extractStream = new MemoryStream(fileBytes))
            {
                content = extractor.Extract(extractStream);
            }

            var lecture = new Lectures()
            {
                CourseId = request.CourseId,
                LecName = request.LectureFile.FileName,
                UploadedAt = DateTime.UtcNow,
                UploadUrl = fileKey,
                FileHash = fileHash,
                Content = content,
                Id = Guid.NewGuid()
            };
            var x = new UploadLectureEvent()
            {
                Content = lecture.Content,
                CourseId = lecture.CourseId,
                LectureId = lecture.Id,
                UserId = currentUser.UserId
            };
          
            await context.Lectures.AddAsync(lecture, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
            await publishEndpoint.Publish(new UploadLectureEvent()
            {
                Content = lecture.Content,
                CourseId = lecture.CourseId,
                LectureId = lecture.Id,
                UserId = currentUser.UserId
            }, cancellationToken);

            return new Revisio.Application.Common.Models.Response<Guid>() { Data = lecture.Id, Success = true, Message = "Uploaded Successfully" };
        }
    }
}

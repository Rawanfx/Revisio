using MediatR;
using Microsoft.EntityFrameworkCore;
using Revisio.Application.Common.Exceptions;
using Revisio.Application.Common.Interfaces;
using Revisio.Application.Common.Models;
using Revisio.Domain.Entities;
using System.Security.Cryptography;

namespace Revisio.Application.PastExam.Command.UploadPastExam
{
    public class UploadPastExamCommandHandler : IRequestHandler<UploadPastExamCommand, Response<Guid>>
    {
        private readonly ICurrentUserService currentUser;
        private readonly IAppDbContext context;
        private readonly IUploadToCloud uploadToCloud;
        public UploadPastExamCommandHandler(ICurrentUserService currentUser
            ,IAppDbContext context
            ,IUploadToCloud uploadToCloud)
        {
            this.context = context;
            this.currentUser = currentUser;
            this.uploadToCloud = uploadToCloud;
        }
        public async Task<Response<Guid>> Handle(UploadPastExamCommand request, CancellationToken cancellationToken)
        {
            var userId = currentUser.UserId;
            if (userId == null)
                throw new UnauthorizedException("Unauthorized user");

            var course = await context.Courses
                .FirstOrDefaultAsync(x => x.Id == request.CourseId && x.UserId == userId, cancellationToken);

            if (course == null)
                throw new NotFoundException("Course not found");

            using var memoryStream = new MemoryStream();
            await request.pastFile.CopyToAsync(memoryStream, cancellationToken);
            memoryStream.Position = 0;

            string fileHash;
            using (var sha256 = SHA256.Create())
            {
                var hashBytes = await sha256.ComputeHashAsync(memoryStream, cancellationToken);
                fileHash = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
            }

            if (await context.PastExams.AnyAsync(x => x.FileHash == fileHash && x.CourseId == request.CourseId, cancellationToken))
                throw new RepeatException("This file has already been uploaded for this course");

            memoryStream.Position = 0;
            var uploadFileKey = await uploadToCloud.UploadAsync(memoryStream, request.pastFile.FileName, request.pastFile.ContentType);

            var pastExam = new PastExams
            {
                CourseId = request.CourseId,
                FileHash = fileHash,
                InstructorName = request.InstName,
                UploadUrl = uploadFileKey
            };

            context.PastExams.Add(pastExam);
            await context.SaveChangesAsync(cancellationToken);

            return new Response<Guid> { Data = pastExam.Id, Success = true, Message = "Exam has been uploaded successfully" };
        }
    }
}

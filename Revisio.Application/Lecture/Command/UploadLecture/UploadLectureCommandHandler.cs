using MediatR;
using Microsoft.EntityFrameworkCore;
using Revisio.Application.Common.Exceptions;
using Revisio.Application.Common.Interfaces;
using Revisio.Application.Common.Models;
using Revisio.Domain.Entities;
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
            using var stream = request.LectureFile.OpenReadStream();
            var fileKey = await uploadToCloud.UploadAsync(stream, request.LectureFile.FileName, request.LectureFile.ContentType);
            var lecture = new Lectures()
            {
                CourseId = request.CourseId,
                LecName = request.LectureFile.FileName,
                UploadedAt = DateTime.UtcNow,
                UploadUrl = fileKey
            };
           await context.Lectures.AddAsync(lecture);
            await context.SaveChangesAsync();
            return new Response<Guid>() {Data= lecture.Id,Success=true, Message="Uploaded Successfully" };
        }
    }
}

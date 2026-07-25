using MediatR;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Revisio.Application.Common.Exceptions;
using Revisio.Application.Common.Interfaces;
using Revisio.Application.Common.Models;

namespace Revisio.Application.Course.Command.AddCourse
{
    public class AddCourseCommandHandler : IRequestHandler<AddCourseCommand, Response<Guid>>
    {
        private readonly IAppDbContext context;
        private readonly ICurrentUserService currentUser;
        public AddCourseCommandHandler (IAppDbContext context, ICurrentUserService currentUser)
        {
            this.context = context;
            this.currentUser = currentUser;
        }
        public async Task<Response<Guid>> Handle(AddCourseCommand request, CancellationToken cancellationToken)
        {
            var user = currentUser.UserId;
            if (user == null)
                throw new UnauthorizedException("Unauthenticated user");
            var course = new Domain.Entities.Course()
            {
                CourseName = request.CourseName,
                InstructorName = request.InstructorName,
                Semester = request.Semesters,
                UserId = user,
            };
           await context.Courses.AddAsync(course);
            await context.SaveChangesAsync();
            return new Response<Guid>() { Success = true, Message = "Course Added Successfully", Data = course.Id };
        }
    }
}

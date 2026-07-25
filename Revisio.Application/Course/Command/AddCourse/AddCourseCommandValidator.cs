using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Revisio.Application.Common.Interfaces;

namespace Revisio.Application.Course.Command.AddCourse
{
    public class AddCourseCommandValidator: AbstractValidator<AddCourseCommand>
    {
        private readonly IAppDbContext _context;
        private readonly ICurrentUserService _currentUserService;

        public AddCourseCommandValidator(IAppDbContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;

            RuleFor(x => x.CourseName)
                .NotEmpty().WithMessage("Course name is required")
                .MaximumLength(200).WithMessage("Course name must not exceed 200 characters")
                .MustAsync(BeUniqueForStudent).WithMessage("You already added this course");


            RuleFor(x => x.InstructorName)
                .NotEmpty().WithMessage("Instructor name is required")
                .MaximumLength(200).WithMessage("Instructor name must not exceed 200 characters");

            RuleFor(x => x.Semesters)
                .IsInEnum().WithMessage("Invalid semester value");
        }
        private async Task<bool> BeUniqueForStudent(AddCourseCommand command, string courseName, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.UserId;
            return !await _context.Courses
                .AnyAsync(c => c.UserId == userId && c.CourseName == courseName, cancellationToken);
        }
    }
}

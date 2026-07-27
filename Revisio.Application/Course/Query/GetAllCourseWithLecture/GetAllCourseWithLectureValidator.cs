using FluentValidation;

namespace Revisio.Application.Course.Query.GetAllCourseWithLecture
{
    public class GetAllCourseWithLectureValidator:AbstractValidator<GetAllCoursesWithLectureQuery>
    {
        public GetAllCourseWithLectureValidator()
        {
            RuleFor(x => x.pageNum)
                .GreaterThanOrEqualTo(1);
            RuleFor(x => x.pageSize)
                .GreaterThanOrEqualTo(1)
                .LessThanOrEqualTo(50);
        }
    }
}

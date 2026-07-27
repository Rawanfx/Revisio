namespace Revisio.Application.Course.Common.Dtos
{
    public class CourseDto
    {
        public Guid CourseId { get; set; }
        public string CourseName { get; set; }
        public string ProfName { get; set; }
        public int LecNum { get; set; }
        public int ExamNum { get; set; }
    }
}

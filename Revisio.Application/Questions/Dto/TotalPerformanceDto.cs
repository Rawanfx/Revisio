namespace Revisio.Application.Questions.Dto
{
    public class TotalPerformanceDto
    {
        public List< CourseAccuracy> CourseAccuracy { get; set; }
    }
    public class CourseAccuracy
    {
        public Guid CourseId { get; set; }
        public string CourseName { get; set; }
        public decimal Score { get; set; }
    }
}

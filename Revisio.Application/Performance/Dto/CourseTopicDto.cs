namespace Revisio.Application.Performance.Dto
{
    public class CourseTopicDto
    {
        public Guid CourseId { get; set; }
        public string CourseName { get; set; }
        public List<Topics> TopicsAccuracy { get; set; }
    }
    public class Topics
    {
        public string Topic { get; set; }
        public decimal Accuracy { get; set; }
    }
}

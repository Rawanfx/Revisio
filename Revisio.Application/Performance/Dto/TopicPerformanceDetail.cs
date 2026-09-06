namespace Revisio.Application.Performance.Dto
{
    public class TopicPerformanceDetail
    {
        public string Topic { get; set; }
        public string LectureName { get; set; }
        public Guid LectureId { get; set; }
        public int MissedCount { get; set; }
        public int TotalAttempted { get; set; }
        public decimal Accuracy { get; set; }
    }
}

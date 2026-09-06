
namespace Revisio.Application.Performance.Dto
{
    public class GeneatePreExamSummaryDto
    {
        public bool success { get; set; }
        public string error_message { get; set; }
        public List<TopicReview> reviews { get; set; }
    }
    public class TopicReview
    {
       public  string topic { get; set; }
       public string lecture_id { get; set; }
      public  string focus_points { get; set; }
    }
}

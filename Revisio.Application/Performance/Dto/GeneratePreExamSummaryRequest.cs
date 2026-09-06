
namespace Revisio.Application.Performance.Dto
{
    public class GeneratePreExamSummaryRequest
    {
      
      public  string course_id { get; set; }
       public List< WeakTopic> weak_topics { get; set; }
    }
    public class WeakTopic
        {
      public  string topic { get; set; }
      public  string lecture_id { get; set; }
        public int missed_count { get; set; }
        public int total_attempted { get; set; }
    }
    
}


using Revisio.Application.Performance.Enums;

namespace Revisio.Application.Performance.Dto
{
    public class WeakReviewDto
    {
        public List<WeakTopicDto> WeakTopicDto { get; set; }
    }
    public class WeakTopicDto
    {
        public string Topic { get; set; }
        public string LectureName { get; set; }
        public string Status { get; set; }
        public decimal Accuracy { get; set; }

    }
}

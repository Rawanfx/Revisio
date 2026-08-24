using Revisio.Domain.Enums;

namespace Revisio.Application.Questions.Dto
{
    public class GradeAnswerResultDto
    {
        public bool Success { get; set; }
        public List<string> Missing_point { get; set; }
        public string Error_message { get; set; }
        public decimal Score { get; set; }
        public string feedback { get; set; }
        public string confidence { get; set; }
    }
}

namespace Revisio.Application.PastExam.Common
{
    public class PastExamDto
    {
        public string UploadUrl { get; set; }
        public string ExamType { get; set; }
        public DateTime UploadDate { get; set; }
        public string? profName { get; set; }
    }
}

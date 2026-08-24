namespace Revisio.Application.Questions.Dto
{
    public class GradeAnswerRequest
    {
        public string Lecture_id { get; set; }
        public string Student_answer { get; set; }
        public string Question_text { get; set; }
        public decimal Max_score { get; set; }
        public string Model_answer { get; set; }
        public string Grading_criteria { get; set; }
    }
}

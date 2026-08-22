using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Revisio.Domain.Entities
{
    public class ExamSessionAnswer
    {
        [Key]
        public Guid Id { get; set; }
        public decimal? Score { get; set; }//for essay , written questions and Mcq
        [ForeignKey (nameof (QuestionOptions))]
        public Guid? UserAnswerOption { get; set; }//for Mcq and true,false
        public QuestionOptions QuestionOptions { get; set; }
        public string? UserAnswerEsaay { get; set; }//for esaay question
        public string? FileKeyUpload { get; set; } //for image answer
        public TimeSpan TimeTakeForAnswer { get; set; }
        [ForeignKey (nameof (ExamSession))]
        public Guid ExamSessionId { get; set; }
        public ExamSession ExamSession { get; set; }
        [ForeignKey(nameof(Questions))]
        public Guid QuestionId { get; set; }
        public Questions Questions { get; set; }
        public bool? IsCorrect { get; set; }
    }
}

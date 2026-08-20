using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Revisio.Domain.Entities
{
    public class ExamSession
    {
        [Key]
        public Guid Id { get; set; }
        [ForeignKey(nameof(ApplicationUser))]
        public string UserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        [ForeignKey(nameof(GenerationRequest))]
        public Guid GenerationRequestId { get; set; }
        public GenerationRequest GenerationRequest { get; set; }
        public DateTime StartAt { get; set; }
        public DateTime? EndAt { get; set; }
        public decimal ? TotalScore { get; set; }
        public int TotalQuestions { get; set; }
        public int CorrectAnswersCount { get; set; }
        public DateTime CompletedAt { get; set; }
        public ICollection<ExamSessionAnswer> ExamSessionAnswers => new List<ExamSessionAnswer>();
    }
}

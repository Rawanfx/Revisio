using Revisio.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Revisio.Domain.Entities
{
    public class Questions
    {
        [Key]
        public Guid Id { get; set; }
        public string Text { get; set; }
        public string Explantion { get; set; }
        public string Topic { get; set; }
        [ForeignKey(nameof(GenerationRequest))]
        public Guid GenrationRequestId { get; set; }
        public QuestionType Type { get; set; }
        public QuestionDifficulty Difficulty { get; set; }
        public GenerationRequest GenerationRequest { get; set; }
        public DateTime CreateAt { get; set; } = DateTime.UtcNow;
        public ICollection<QuestionOptions> QuestionOptions { get; set; } = new List<QuestionOptions>();
    }
}

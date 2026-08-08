
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Revisio.Domain.Entities
{
    public class QuestionOptions
    {
        [Key]
        public Guid Id { get; set; }
        [ForeignKey(nameof(Questions))]
        public Guid QuestionId { get; set; }
        public Questions Questions { get; set; }
        public string Option { get; set; }
        public bool IsCorrect { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Revisio.Domain.Entities
{
    public class PastExams
    {
        [Key]
        public Guid Id { get; set; }
        public string UploadUrl { get; set; }
        [ForeignKey(nameof(Course))]
        public Guid CourseId { get; set;}
        public Course Course { get; set; }
        public string? InstructorName { get; set; }
    }
}

using Revisio.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Revisio.Domain.Entities
{
    public class Course
    {
        [Key]
        public Guid Id { get; set; }
        public string CourseName { get; set; }
        public string InstructorName { get; set; }
        public Semesters Semester { get; set; }
        public DateTime CreationAt { get; set; } = (DateTime.UtcNow);
        [ForeignKey (nameof (ApplicationUser))]
        public string UserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public List<PastExams> PastExams { get; set; }
        public List<Lectures> Lectures { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Revisio.Domain.Entities
{
    public class Lectures
    {
        [Key]
        public Guid Id { get; set; }
        public string LecName { get; set; }
        public DateTime UploadedAt { get; set; }
        public string UploadUrl { get; set; }
        public string FileHash { get; set; }
        [ForeignKey(nameof (Course))]
        public Guid CourseId { get; set; }
        public Course Course { get; set; }
    }
}

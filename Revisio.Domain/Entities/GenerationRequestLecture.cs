using System.ComponentModel.DataAnnotations.Schema;

namespace Revisio.Domain.Entities
{
    public class GenerationRequestLecture
    {
        [ForeignKey(nameof(GenerationRequest))]
        public Guid GenerationRequestId { get; set; }
        public GenerationRequest GenerationRequest { get; set; }
        [ForeignKey(nameof(Lectures))]
        public Guid LectureId { get; set; }
        public Lectures Lectures { get; set; }
    }
}

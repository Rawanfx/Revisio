namespace Revisio.Application.Events
{
    public record UploadLectureEvent
    {
        public string UserId { get; init; }
        public Guid LectureId { get; init; }
        public string Content { get; init; }
        public Guid CourseId { get; init; }
    }
}

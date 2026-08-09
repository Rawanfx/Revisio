namespace Revisio.Application.Events
{
    public record QuestionGeneratedRequest
    {
        public Guid GenerateRequestId { get; init; }
    }
}

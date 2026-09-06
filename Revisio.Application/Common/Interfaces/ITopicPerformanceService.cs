using Revisio.Application.Performance.Dto;

namespace Revisio.Application.Common.Interfaces
{
    public interface ITopicPerformanceService
    {
        Task<TopicPerformanceDetail> TopicPerformance(Guid courseId, string userId,CancellationToken cancellationToken);
    }
}

using MediatR;
using Revisio.Application.Common.Interfaces;
using Revisio.Application.Common.Models;
using Revisio.Application.Performance.Dto;
namespace Revisio.Application.Performance.Query.GeneratePersonalizedSummary
{
    public class GeneratePersonalizedSummaryQueryHandler : IRequestHandler<GeneratePersonalizedSummaryQuery, Response<TopicPerformanceDetail>>
    {
        private readonly ITopicPerformanceService topicPerformanceService;
        private readonly ICurrentUserService userService;
        public GeneratePersonalizedSummaryQueryHandler(ITopicPerformanceService topicPerformanceService,
            ICurrentUserService userService)
        {
            this.topicPerformanceService = topicPerformanceService;
            this.userService = userService;
        }
        public async Task<Response<TopicPerformanceDetail>> Handle(GeneratePersonalizedSummaryQuery request, CancellationToken cancellationToken)
        {
            var allTopics = await topicPerformanceService.TopicPerformance(request.CourseId, userService.UserId, cancellationToken);

            if (!allTopics.Any())
                return new Response<TopicPerformanceDetail>() { Success = true, Message = "No Mistacks!" };

        }
    }
}

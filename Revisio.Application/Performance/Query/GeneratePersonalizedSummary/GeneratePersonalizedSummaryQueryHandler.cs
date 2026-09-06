using MediatR;
using Revisio.Application.Common.Interfaces;
using Revisio.Application.Common.Models;
using Revisio.Application.Performance.Dto;
namespace Revisio.Application.Performance.Query.GeneratePersonalizedSummary
{
    public class GeneratePersonalizedSummaryQueryHandler : IRequestHandler<GeneratePersonalizedSummaryQuery, Response<GeneatePreExamSummaryDto>>
    {
        private readonly ITopicPerformanceService topicPerformanceService;
        private readonly ICurrentUserService userService;
        private readonly IExamAIGenerator aiService;
        public GeneratePersonalizedSummaryQueryHandler(ITopicPerformanceService topicPerformanceService,
            ICurrentUserService userService
            ,IExamAIGenerator aiService)
        {
            this.topicPerformanceService = topicPerformanceService;
            this.userService = userService;
            this.aiService = aiService;
        }
        public async Task<Response<GeneatePreExamSummaryDto>> Handle(GeneratePersonalizedSummaryQuery request, CancellationToken cancellationToken)
        {
            var allTopics = await topicPerformanceService.TopicPerformance(request.CourseId, userService.UserId, cancellationToken);

            if (!allTopics.Any())
                return new Response<GeneatePreExamSummaryDto>() { Success = true, Message = "No Mistacks!" };
            var weakTopics = allTopics.Select(x => new WeakTopic()
            {
                lecture_id=x.LectureName,
                missed_count=x.MissedCount,
                topic=x.Topic,
                  total_attempted=x.TotalAttempted
            }).ToList();
            var generateRequest = new GeneratePreExamSummaryRequest() { 
                course_id= request.CourseId.ToString(),
                weak_topics= weakTopics
            };
            var result = await aiService.GeneratePreExamSummary(generateRequest, cancellationToken);
            return new Response<GeneatePreExamSummaryDto>() { Data = result, Success = true };
        }
    }
}

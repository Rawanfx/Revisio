using MediatR;
using Microsoft.EntityFrameworkCore;
using Revisio.Application.Common.Interfaces;
using Revisio.Application.Common.Models;
using Revisio.Application.Performance.Dto;
namespace Revisio.Application.Performance.Query.WeakTopics
{
    public class WeakTopicQueryHandler : IRequestHandler<WeakTopicQuery, Response<WeakReviewDto>>
    {
        private readonly IAppDbContext context;
        private readonly ICurrentUserService userService;
        private readonly ITopicPerformanceService topicPerformanceService;
        public WeakTopicQueryHandler (IAppDbContext context
            , ICurrentUserService userService
            ,ITopicPerformanceService topicPerformanceService)
        {
            this.userService = userService;
            this.context = context;
            this.topicPerformanceService = topicPerformanceService;
        }
        public async Task<Response<WeakReviewDto>> Handle(WeakTopicQuery request, CancellationToken cancellationToken)
        {
            var response = await topicPerformanceService.TopicPerformance(request.CourseId, userService.UserId, cancellationToken);

            var weakTopics = response
                .Where(x=>x.Accuracy<70)
                .Select(x => new WeakTopicDto()
            {
                 Accuracy = x.Accuracy,
                 LectureName = x.LectureName,
                  Status = x.Accuracy<50?"Weak":"Review"
            }).ToList();
            return new Response<WeakReviewDto>()
            {
                Success = true,
                Data = new WeakReviewDto() { WeakTopicDto = weakTopics }
            };
        }
    }
}

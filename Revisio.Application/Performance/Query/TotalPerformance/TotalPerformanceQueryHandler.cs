
using MediatR;
using Microsoft.EntityFrameworkCore;
using Revisio.Application.Common.Interfaces;
using Revisio.Application.Common.Models;
using Revisio.Application.Performance.Dto;

namespace Revisio.Application.Performance.Query.TotalPerformance
{
    public class TotalPerformanceQueryHandler : IRequestHandler<TotalPerformanceQuery, Response<TotalPerformanceDto>>
    {
        private readonly ICurrentUserService userService;
        private readonly IAppDbContext context;
        public TotalPerformanceQueryHandler(ICurrentUserService userService,IAppDbContext context)
        {
            this.context = context;
            this.userService = userService;
        }
        public async Task<Response<TotalPerformanceDto>> Handle(TotalPerformanceQuery request, CancellationToken cancellationToken)
        {
            var generatedRequest = await context.GenerationRequests
                 .Where(x => x.UserId == userService.UserId && x.GenrateExamStatus == Domain.Enums.GenrateExamStatus.Completed
                 && x.ExamSession.EndAt != null)
                 .Select(x => new 
                 {
                   
                     CourseId = x.CourseId,
                     CourseName = x.Course.CourseName,
                     Score = x.ExamSession.TotalScore??0,
                     MaxScore =x.ExamSession.TotalMaxScore
                 }).ToListAsync();
            var courseAccuracy = generatedRequest
                 .GroupBy(x => x.CourseId)
                 .Select(y => new CourseAccuracy() {
                     Score=y.Sum(x=>x.Score)>0?Math.Round((y.Sum(x => x.Score) / y.Sum(x => x.MaxScore)) * 100, 0) : 0,
                     CourseId = y.Select(z=>z.CourseId).FirstOrDefault(),
                     CourseName=y.Select(z=>z.CourseName).FirstOrDefault(),
                 }).ToList();
            return new Response<TotalPerformanceDto>()
            {
                Success = true,
                Data = new TotalPerformanceDto() { CourseAccuracy = courseAccuracy }
            };
        }
    }
}

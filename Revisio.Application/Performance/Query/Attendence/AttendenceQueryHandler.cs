using MediatR;
using Microsoft.EntityFrameworkCore;
using Revisio.Application.Common.Interfaces;
using Revisio.Application.Common.Models;
using Revisio.Application.Performance.Dto;
namespace Revisio.Application.Performance.Query.Attendence
{
    public class AttendenceQueryHandler : IRequestHandler<AttendenceQuery, Response<AttendenceDto>>
    {
        private readonly IAppDbContext context;
        private readonly ICurrentUserService userService;
        public AttendenceQueryHandler (IAppDbContext context,ICurrentUserService userService)
        {
            this.context = context;
            this.userService = userService;
        }
        public async Task<Response<AttendenceDto>> Handle(AttendenceQuery request, CancellationToken cancellationToken)
        {
            var scores = await context.ExamSessionAnswers
                 .Include(x => x.Questions)
                 .ThenInclude(x => x.GenerationRequest)
                 .Where(x => x.Questions.GenerationRequest.CourseId == request.CourseId
                 && x.Questions.GenerationRequest.UserId == userService.UserId)
                 .Select(x => new
                 {
                   studentStudent=  x.Score,
                   questionAnswer = x.Questions.MaxScore
                 }).ToListAsync();
            var questionNum = scores.Count;
            var accuracy = Math.Round((scores.Sum(x => x.studentStudent ?? 0) / scores.Sum(x => x.questionAnswer)) / 100, 2);
            return new Response<AttendenceDto>()
            {
                Success = true,
                Data = new AttendenceDto() { Accuracy = accuracy, QuestionNum = questionNum }
            };
        }
    }
}

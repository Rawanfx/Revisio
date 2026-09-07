using Revisio.Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Revisio.Application.Common.Exceptions;
using Revisio.Application.Common.Interfaces;
using Revisio.Application.ExplainMore.Dto;

namespace Revisio.Application.ExplainMore.Query
{
    public class ExplainSelectedTextQueryHandler : IRequestHandler<ExplainSelectedTextQuery,Response< ExplainTextResponse>>
    {
        private readonly IAppDbContext context;
        private readonly ICurrentUserService userService;
        private readonly IExamAIGenerator aiService;
        public ExplainSelectedTextQueryHandler (IAppDbContext context,ICurrentUserService userService,IExamAIGenerator aiService)
        {
            this.userService = userService;
            this.context = context;
            this.aiService = aiService;
        }
        public async Task<Response<ExplainTextResponse>> Handle(ExplainSelectedTextQuery request, CancellationToken cancellationToken)
        {
            var examSession = await context.ExamSessions
                .FirstOrDefaultAsync(x => request.ExamSessionId == x.Id && x.UserId == userService.UserId );
            if (examSession == null)
                throw new NotFoundException("Session not found");
            var question = await context.ExamSessionAnswers
                .Include (x=>x.Questions)
                .FirstOrDefaultAsync(x => x.QuestionId == request.QuestionId && x.ExamSessionId == request.ExamSessionId);
            if (question == null)
                throw new NotFoundException("question not found");

            var response = await aiService.GenerateExplainText(question.Questions.LectureId.ToString()
                , request.SelectedConcept, question.Questions.Text, cancellationToken);
            return new Response<ExplainTextResponse>() { Success = true, Data = response };
        }
    }
}

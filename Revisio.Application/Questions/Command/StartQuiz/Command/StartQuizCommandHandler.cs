using MassTransit;
using MassTransit.Initializers;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Revisio.Application.Common.Exceptions;
using Revisio.Application.Common.Interfaces;
using Revisio.Application.Questions.Dto;
using Revisio.Domain.Entities;
using System.Runtime.InteropServices;

namespace Revisio.Application.Questions.Command.StartQuiz.Command
{
    public class StartQuizCommandHandler : IRequestHandler<StartQuizCommand,Common.Models. Response<StartQuizResponse>>
    {
        private readonly IAppDbContext context;
        private readonly ICurrentUserService currentUser;
        public StartQuizCommandHandler(IAppDbContext context
            ,ICurrentUserService currentUser)
        {
            this.currentUser = currentUser;
            this.context = context;
        }
        public async Task<Common.Models.Response<StartQuizResponse>> Handle(StartQuizCommand request, CancellationToken cancellationToken)
        {
            var generationRequest = await context.GenerationRequests
                .FirstOrDefaultAsync(x => x.UserId == currentUser.UserId && x.Id==request.GenerationRequestId);
            if (generationRequest == null)
                throw new NotFoundException("Generation Request not found");
            var examSession = new ExamSession()
            {
                UserId = currentUser.UserId,
                Id = Guid.NewGuid(),
                GenerationRequestId = request.GenerationRequestId,
                StartAt = DateTime.UtcNow,
                TotalQuestions = generationRequest.TotalQuestions,
            };
            context.ExamSessions.Add(examSession);
            await context.SaveChangesAsync(cancellationToken);
            //get first question from questions table with index =1
            StartQuizResponse startQuizResponse = new StartQuizResponse()
            {
                ExamSessionId = examSession.Id
            };
            var firstq = await context.Questions
                 .Where(x => x.GenerationRequestId == request.GenerationRequestId && x.Index == 1)
                 .Select(y => new
                 {
                     y.Id,
                     y.Text,
                    options= y.QuestionOptions.Select(z=>new {z.Id,z.Option}).ToList(),
                     y.Type
                 }).FirstOrDefaultAsync(cancellationToken);
            if (firstq is null)
                throw new NotFoundException("No questions found for this generation request");

            questionData questionData = new questionData()
            {
                Index = 1,
                QuestionType=firstq.Type,
                QuestionId=firstq.Id,
               Options=firstq.options.ToDictionary(x=>x.Id,x=>x.Option),
               Text=firstq.Text
            };

            startQuizResponse.questionData = questionData;
            return new Common.Models.Response<StartQuizResponse>() { Success = true,Data=startQuizResponse,Message="Quiz Started" };

        }
    }
}

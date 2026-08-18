using MassTransit;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Revisio.Application.Common.Exceptions;
using Revisio.Application.Common.Interfaces;
using Revisio.Application.Common.Models;
using Revisio.Application.Events;
using Revisio.Domain.Entities;
using Revisio.Domain.Enums;
using System.Diagnostics;

namespace Revisio.Application.Questions.Command.GenerateQuestion
{
    public class GenerateQuestionCommandHandler : IRequestHandler<GenerateQuestionCommand, Revisio.Application.Common.Models.Response<Guid>>
    {
        private readonly ICurrentUserService currentUserService;
        private readonly IAppDbContext context;
        private readonly IPublishEndpoint publishEndpoint;
        public GenerateQuestionCommandHandler( ICurrentUserService currentUserService
            ,IAppDbContext context
            ,IPublishEndpoint publishEndpoint)
        {
            this.context = context;
            this.publishEndpoint = publishEndpoint;
            this.currentUserService = currentUserService;
        }
        public async Task<Revisio.Application.Common.Models.Response<Guid>> Handle(GenerateQuestionCommand request, CancellationToken cancellationToken)
        {
            var lecIds = await context.Lectures
                .Where(x => request.GenerateQuestionRequestDto.LectureIds.Contains(x.Id)
                && x.Course.UserId == currentUserService.UserId)
                .Select(x => x.Id)
                .ToListAsync(cancellationToken);
            if (lecIds.Count != request.GenerateQuestionRequestDto.LectureIds.Count)
                throw new NotFoundException("Lectures not found");
            var generationRequest = new GenerationRequest()
            {
                Id = Guid.NewGuid(),
                UserId = currentUserService.UserId,
                EasyQuestionNum = request.GenerateQuestionRequestDto.Easy,
                EssayQuestionNum = request.GenerateQuestionRequestDto.Essay,
                ExamMode = request.GenerateQuestionRequestDto.ExamMode,
                GenrateExamStatus = GenrateExamStatus.Pending,
                HardQuestionNum = request.GenerateQuestionRequestDto.Hard,
                MCQQuestionNum = request.GenerateQuestionRequestDto.MCQ,
                MediumQuestionNum = request.GenerateQuestionRequestDto.Medium,
                TotalQuestions = request.GenerateQuestionRequestDto.TotalQuestions,
                TrueFalseQuestionNum = request.GenerateQuestionRequestDto.TrueFalse
            };
            List<GenerationRequestLecture> generationRequestLecture = new();
            foreach (var i in request.GenerateQuestionRequestDto.LectureIds)
            {
                generationRequestLecture.Add(new GenerationRequestLecture()
                {
                    LectureId=i,
                    GenerationRequestId=generationRequest.Id
                });
            }
           
            await context.GenerationRequests.AddAsync(generationRequest);
            await context.GenerationRequestLectures.AddRangeAsync(generationRequestLecture);
            
            await publishEndpoint.Publish(new QuestionGeneratedRequest
            {
                GenerateRequestId = generationRequest.Id
            });
            await context.SaveChangesAsync();
            return new Common.Models.Response<Guid>() { Success = true, Data = generationRequest.Id };

        }
    }
}

using MassTransit;
using Microsoft.EntityFrameworkCore;
using Revisio.Application.Common.Interfaces;
using Revisio.Application.Events;

namespace Revisio.Infrastructure.Consumers
{
    public class QuestionGenerationConsumer : IConsumer<QuestionGeneratedRequest>
    {
        private readonly IGenerateQuestionAIService aiServiceClient;
        private readonly IAppDbContext dbContext;
        public QuestionGenerationConsumer(IGenerateQuestionAIService aiServiceClient
            , IAppDbContext dbContext)
        {
            this.aiServiceClient = aiServiceClient;
            this.dbContext = dbContext;
        }
        public async Task Consume(ConsumeContext<QuestionGeneratedRequest> context)
        {
            // var generatedQuestionId = context.Message.GenerateRequestId;
            //var generatedQuestion = await dbContext.GenerationRequests.FirstOrDefaultAsync(x => x.Id == generatedQuestionId);
            //if (generatedQuestion == null)
            //    Log.Error($"generatedQuestionId Not Found - ID = {generatedQuestionId}");
            // r selecteLecture 
            throw new Exception();
        }
    }
}

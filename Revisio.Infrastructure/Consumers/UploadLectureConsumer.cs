using MassTransit;
using Revisio.Application.Common.Interfaces;
using Revisio.Application.Events;

namespace Revisio.Infrastructure.Consumers
{
    public class UploadLectureConsumer : IConsumer<UploadLectureEvent>
    {
        private readonly IAppDbContext dbContext;
        private IExamAIGenerator examAIGenerator;
        public UploadLectureConsumer(IAppDbContext dbContext,IExamAIGenerator examAIGenerator)
        {
            this.dbContext = dbContext;
            this.examAIGenerator = examAIGenerator;
        }
        public async Task Consume(ConsumeContext<UploadLectureEvent> context)
        {
            var message = context.Message;
            try
            {
                var response = await examAIGenerator.IndexLectureAsync(message.Content, message.UserId, message.LectureId, message.CourseId, context.CancellationToken);
                var lecture = await dbContext.Lectures.FindAsync(message.LectureId);
                if (lecture != null)
                {
                    lecture.IndexingStatus = response ? IndexingStatus.Indexed : IndexingStatus.Failed;
                    await dbContext.SaveChangesAsync(context.CancellationToken);
                }

            }
            catch(Exception ex)
            {
                var lecture = await dbContext.Lectures.FindAsync(message.LectureId);
                if (lecture != null)
                {
                    lecture.IndexingStatus = IndexingStatus.Failed;
                    await dbContext.SaveChangesAsync(context.CancellationToken);
                }
                throw;
            }
        }
    }
}

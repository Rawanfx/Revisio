using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Revisio.Application.Common.Interfaces;
using Revisio.Application.Questions.Command.GenerateQuestion;
using Revisio.Application.Questions.Command.StartQuiz.Command;
using Revisio.Application.Questions.Command.SubmitAnswer;
using Revisio.Domain.Entities;

namespace Revisio.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionsController : ControllerBase
    {
        private readonly IMediator mediator;
        public QuestionsController(IMediator mediator)
        {
            this.mediator = mediator;
        }
        [HttpPost()]
        [Authorize(Roles ="Student")]
        public async Task<IActionResult>GenerateQuestions ([FromBody]GenerateQuestionCommand command)
        {
            var result = await mediator.Send(command);
            return Created(string.Empty,result);
        }
        [HttpPost("exam-sessions")]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> StartQuiz([FromBody] StartQuizCommand command)
        {
            var result = await mediator.Send(command);
            return Ok(result);
        }
        [HttpPost("submit-answer")]
        [Authorize(Roles = "Student")]
        public async Task <IActionResult>SubmitAndNextQuestions (SubmitAndNextQuestionCommand request)
        {
            var result = await mediator.Send(request);
            return Ok(result);
        }
        [HttpGet("test-insert")]
        public async Task<IActionResult> TestInsert([FromServices] IAppDbContext context)
        {
            var testAnswer = new ExamSessionAnswer
            {
                Id = Guid.NewGuid(),
                ExamSessionId = Guid.Parse("2ff3bd1c-01f0-4a75-b22d-2c7c3709a2ce"),
                QuestionId = Guid.Parse("e5ff6231-35bb-4604-90cb-73f93da01745"),
                Score = 5,
                TimeTakeForAnswer = TimeSpan.FromSeconds(45),
                UserAnswerEsaay = null,
                UserAnswerOption = Guid.Parse("63432C3A-0755-41B0-BEEA-49F124A64C7C")
            };

            context.ExamSessionAnswers.Add(testAnswer);
            Console.WriteLine($"State: {context.Entry(testAnswer).State}");

            var rows = await context.SaveChangesAsync();

            return Ok(new { rows, state = context.Entry(testAnswer).State.ToString() });
        }
      
    }
}

using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Revisio.Application.Questions.Command.GenerateQuestion;
using Revisio.Application.Questions.Command.StartQuiz;
using Revisio.Application.Questions.Command.SubmitAnswer;

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
    }
}

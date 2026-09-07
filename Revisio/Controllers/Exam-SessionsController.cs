using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Revisio.Application.ExplainMore.Dto;
using Revisio.Application.ExplainMore.Query;
using Revisio.Application.Performance.Query.ExamResult;

namespace Revisio.API.Controllers
{
    [Route("api/exam-sessions")]
    [ApiController]
    public class ExamSessionsController
        : ControllerBase
    {
        private readonly IMediator mediator;
        public ExamSessionsController(IMediator mediator)
        {
            this.mediator = mediator;
        }
        [HttpPost("{examSessionId}/questions/{questionId}/explain")]
        [Authorize(Roles ="Student")]
        public async Task<IActionResult>SelectToExplain (Guid examSessionId,Guid questionId, [FromBody] ExplainSelectedTextRequest request)
        {
            var query = new ExplainSelectedTextQuery(questionId, examSessionId, request.SelectedText);
            var response = await mediator.Send(query);
            return Ok(response);
        }
    }
}

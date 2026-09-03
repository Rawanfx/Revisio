using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Revisio.Application.Performance.Query.ExamResult;

namespace Revisio.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExamSessionsController
        : ControllerBase
    {
        private readonly IMediator mediator;
        public ExamSessionsController(IMediator mediator)
        {
            this.mediator = mediator;
        }
        [HttpGet("{examSessionId}/result")]
        [Authorize(Roles ="Student")]
        public async Task<IActionResult> ExamResult ([FromRoute]ExamResultQuery query)
        {
           var response = await mediator.Send(query);
            return Ok(response);
        }
    }
}

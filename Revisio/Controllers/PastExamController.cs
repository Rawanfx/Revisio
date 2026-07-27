using MediatR;
using Microsoft.AspNetCore.Authorization;
using Revisio.Application.PastExam.Command.UploadPastExam;
using Microsoft.AspNetCore.Mvc;

namespace Revisio.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PastExamController : ControllerBase
    {
        private readonly IMediator mediator;
        public PastExamController(IMediator mediator) => this.mediator = mediator;
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Upload([FromForm]UploadPastExamCommand command)
        {
            var result = await mediator.Send(command);
            return Ok(result);
        }
    }
}

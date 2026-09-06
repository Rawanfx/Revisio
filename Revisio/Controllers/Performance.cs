
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Revisio.Application.Performance.Query.Attendence;
using Revisio.Application.Performance.Query.CourseTopicPERFORMANCE;
using Revisio.Application.Performance.Query.ExamResult;
using Revisio.Application.Performance.Query.WeakTopics;
namespace Revisio.API.Controllers
{
    [Controller]
    [Route("api/[controller]")]
    public class Performance:ControllerBase
    {
        private readonly IMediator mediator;
        public Performance (IMediator mediator) { this.mediator = mediator; }
        [HttpGet("course/{courseId}/topics")]
        [Authorize(Roles ="Student")]
        public async Task< IActionResult > CoursePerformance(Guid courseId)
        {
            var response = await mediator.Send(new CourseTopicQuery(courseId));
            return Ok(response);
        }
        [HttpGet("result/{ExamSessionId}")]
        [Authorize(Roles = "Student")]
        public async Task <IActionResult>ExamResult([FromRoute] ExamResultQuery query)
        {
            var response = await mediator.Send(query);
            return Ok(response);
        }
        [HttpGet("{courseId}/needs-review")]
        [Authorize (Roles ="Student")]
        public async Task<IActionResult> Review([FromRoute]WeakTopicQuery query)
        {
            var response = await mediator.Send(query);
            return Ok(response);
        }
        [HttpGet("{courseId}/attendence")]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult>Attendence([FromRoute] AttendenceQuery query)
        {
            var response = await mediator.Send(query);
            return Ok(response);
        }
    }
}


using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Revisio.Application.Performance.CourseTopicPERFORMANCE;
using Revisio.Application.Performance.Dto;
using Revisio.Application.Common.Models;
namespace Revisio.API.Controllers
{
    [Controller]
    [Route("api/[controller]")]
    public class Performance:ControllerBase
    {
        private readonly IMediator mediator;
        public Performance (IMediator mediator) { this.mediator = mediator; }
        [HttpGet("/course/{courseId}/topics")]
        [Authorize(Roles ="Student")]
        public async Task< IActionResult > CoursePerformance(Guid courseId)
        {
            var response = await mediator.Send(new CourseTopicQuery(courseId));
            return Ok(response);
        }
    }
}

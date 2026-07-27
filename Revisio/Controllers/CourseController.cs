using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Revisio.Application.Course.Command.AddCourse;
using Revisio.Application.Course.Query.GetAllCourseWithLecture;

namespace Revisio.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        private readonly IMediator mediator;
        public CourseController(IMediator mediator) => this.mediator = mediator;
        
        [HttpPost]
        [Authorize(Roles ="Student")]
        public async Task<IActionResult>AddCourse (AddCourseCommand command)
        {
           var response= await mediator.Send(command);
            return Ok(response);
        }
        [HttpGet("")]
        [Authorize (Roles ="Student")]
        public async Task <ActionResult>AllCourse ([FromQuery]GetAllCoursesWithLectureQuery query)
        {
            var response = await mediator.Send(query);
            return Ok( response);
        }
    }
}

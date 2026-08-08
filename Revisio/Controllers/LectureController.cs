using Amazon.S3.Model.Internal.MarshallTransformations;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Revisio.Application.Lecture.Command.UploadLecture;
using Revisio.Application.Lecture.Query.GetAllLecturesForCourse;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Revisio.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LectureController : ControllerBase
    {
        private readonly IMediator mediator;
        public LectureController (IMediator mediator)
        {
            this.mediator = mediator;
        }
        [Authorize()]
        [HttpPost()]
        public async Task<IActionResult> Upload([FromForm] UploadLectureCommand command)
        {
           var result =  await mediator.Send(command);
            return Ok(result);
        }
        [HttpGet("")]
        [Authorize(Roles ="Student")]
        public async Task<IActionResult> GetLecture([FromQuery] GetAllLecturesForCourseQuery query)
        {
            var result = await mediator.Send(query);
            return Ok(result);
        }
    }
}

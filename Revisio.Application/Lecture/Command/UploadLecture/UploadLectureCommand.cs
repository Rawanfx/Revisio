using MediatR;
using Microsoft.AspNetCore.Http;
using Revisio.Application.Common.Models;

namespace Revisio.Application.Lecture.Command.UploadLecture;

public record UploadLectureCommand (Guid CourseId,IFormFile LectureFile):IRequest<Response<Guid>>;

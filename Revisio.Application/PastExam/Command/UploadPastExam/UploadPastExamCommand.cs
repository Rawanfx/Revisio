using MediatR;
using Microsoft.AspNetCore.Http;
using Revisio.Application.Common.Models;

namespace Revisio.Application.PastExam.Command.UploadPastExam;

public record UploadPastExamCommand(Guid CourseId,string InstName,IFormFile pastFile)
    :IRequest<Response<Guid>>;

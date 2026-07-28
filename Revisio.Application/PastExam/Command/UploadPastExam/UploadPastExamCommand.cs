using MediatR;
using Microsoft.AspNetCore.Http;
using Revisio.Application.Common.Models;
using Revisio.Domain.Enums;

namespace Revisio.Application.PastExam.Command.UploadPastExam;

public record UploadPastExamCommand(Guid CourseId,string InstName,IFormFile pastFile,ExamType ExamType)
    :IRequest<Response<Guid>>;

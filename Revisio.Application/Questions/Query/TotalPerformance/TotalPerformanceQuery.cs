using MediatR;
using Revisio.Application.Common.Models;
using Revisio.Application.Questions.Dto;
namespace Revisio.Application.Questions.Query.TotalPerformance;

public record TotalPerformanceQuery:IRequest<Response<TotalPerformanceDto>>;


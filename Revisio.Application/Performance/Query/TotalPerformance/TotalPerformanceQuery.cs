using MediatR;
using Revisio.Application.Common.Models;
using Revisio.Application.Performance.Dto;

namespace Revisio.Application.Performance.Query.TotalPerformance;

public record TotalPerformanceQuery:IRequest<Response<TotalPerformanceDto>>;


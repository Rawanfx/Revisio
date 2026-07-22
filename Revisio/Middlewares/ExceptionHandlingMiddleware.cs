using Revisio.Application.Common.Exceptions;
using Revisio.Application.Common.Models;
using Serilog;
using System;
namespace Revisio.API.Middlewares
{
    public class ExceptionHandlingMiddleware:IMiddleware
    {

        public  ExceptionHandlingMiddleware() { }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch(Exception ex)
            {
                Log.Error($"Exception Thrown {ex.Message} ");
                await HandleException(context, ex);
            }
        }
        private async Task HandleException (HttpContext context,Exception ex)
        {
            var (response, statusCode) = ex switch
            {
                IdentityException exception => (Response<object>.FailResponse("Registratio faild", exception.Errors), StatusCodes.Status400BadRequest),

                NotFoundException exception => (Response<object>.FailResponse(ex.Message),
                       StatusCodes.Status404NotFound),

                UnauthorizedException exception => (Response<object>.FailResponse(ex.Message),
                    StatusCodes.Status401Unauthorized),

                _ => (Response<object>.FailResponse("An Error Occured"),StatusCodes.Status500InternalServerError)
            };
            response.Success = false;
            context.Response.StatusCode = statusCode;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsJsonAsync(response);
        }
    }
}

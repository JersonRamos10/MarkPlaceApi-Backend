
using MarketPlaceApi.Business.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace MarketPlaceApi.Api.Handlers
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly IWebHostEnvironment _env;

        public GlobalExceptionHandler(IWebHostEnvironment env)
        {
            _env = env;
        }
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {   
            
            httpContext.Response.ContentType = "application/json";

            var problemDetails = new ProblemDetails();

            switch (exception)
            {
                case NotFoundException: 
                    httpContext.Response.StatusCode = StatusCodes.Status404NotFound;
                    problemDetails.Detail = exception.Message;
                    problemDetails.Status =  StatusCodes.Status404NotFound;;
                    problemDetails.Title = "Not Found";
                break;

                case BusinessValidationException validationEx: 
                    httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                    problemDetails.Detail = validationEx.Message;
                    problemDetails.Status = StatusCodes.Status400BadRequest;
                    problemDetails.Title = "Validation incorrect";
                    problemDetails.Extensions["field"] = validationEx.FieldName;
                break;

                case ConflictException :
                    httpContext.Response.StatusCode = StatusCodes.Status409Conflict;
                    problemDetails.Detail = exception.Message;
                    problemDetails.Status =StatusCodes.Status409Conflict;;
                    problemDetails.Title = "Conflict";
                break;

                case ForbiddenException :
                    httpContext.Response.StatusCode = StatusCodes.Status403Forbidden;
                    problemDetails.Detail = exception.Message;
                    problemDetails.Status = StatusCodes.Status403Forbidden;
                    problemDetails.Title = "Forbidden";
                    
                break;

                case UnauthorizedException :
                    httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    problemDetails.Detail = exception.Message;
                    problemDetails.Status = StatusCodes.Status401Unauthorized;;
                    problemDetails.Title = "Unauthorized";
                break;
                
                default:
                    httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    problemDetails.Status = StatusCodes.Status500InternalServerError;
                    problemDetails.Title = "Internal Server Error";
                    problemDetails.Detail = _env.IsDevelopment() 
                        ? exception.Message 
                        : "An unexpected error occurred";
                break;
            }


            await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

            
            return true;
        }
    }
}